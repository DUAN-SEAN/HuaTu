using SVGHelper;
using SVGHelper.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace DrawWork
{
    public class DrawConnectLine : DrawObject
    {
        #region 字段

        private const string Tag = "Connectline";

        private PointF _endPoint;
        private PointF _middlePoint;
        private PointF _startPoint;

        private DrawObject _startObject;
        private int _startobjecthandle;
        private DrawObject _endObject;
        private int _endobjecthandle;

        #endregion 字段

        #region 构造器

        public DrawConnectLine()
        {
            _startPoint.X = 0;
            _startPoint.Y = 0;
            _middlePoint.X = 0.5f;
            _middlePoint.Y = 0.5f;
            _endPoint.X = 1;
            _endPoint.Y = 1;

            Initialize();
        }

        public DrawConnectLine(float x1, float y1, float x2, float y2)
        {
            _startPoint.X = x1;
            _startPoint.Y = y1;
            _endPoint.X = x2;
            _endPoint.Y = y2;
            _middlePoint.X = (x1 + x2) / 2;
            _middlePoint.Y = (y1 + y2) / 2;

            Initialize();
        }

        #endregion 构造器

        #region 属性

        public override int HandleCount
        {
            get
            {
                return 3;
            }
        }

        protected GraphicsPath AreaPath
        {
            get; set;
        }

        protected Pen AreaPen
        {
            get; set;
        }

        protected Region AreaRegion
        {
            get; set;
        }

        #endregion 属性

        #region 函数

        public static DrawLineObject Create(SVGLine svg)
        {
            try
            {
                var dobj = new DrawLineObject(ParseSize(svg.X1, Dpi.X),
                    ParseSize(svg.Y1, Dpi.Y),
                    ParseSize(svg.X2, Dpi.X),
                    ParseSize(svg.Y2, Dpi.Y));
                dobj.SetStyleFromSvg(svg);
                return dobj;
            }
            catch (Exception ex)
            {
                SVGErr.Log("CreateLine", "Draw", ex.ToString(), SVGErr._LogPriority.Info);
                return null;
            }
        }

        public override void DrawTracker(Graphics g)
        {
            if (!Selected)
                return;

            var brush = new SolidBrush(Color.Black);

            for (int i = 1; i <= HandleCount; i++)
            {
                try
                {
                    g.FillRectangle(brush, GetHandleRectangle(i));
                }
                catch
                { }
            }

            brush.Dispose();
        }


        public override void Draw(Graphics g)
        {
            try
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                var pen = new Pen(Stroke, StrokeWidth);
                //if(_startPoint.X > _endPoint.X)
                //{
                //    var temp = _startPoint;
                //    _startPoint = _endPoint;
                //    _endPoint = temp;
                //}
                //g.DrawLine(pen, _startPoint.X, _startPoint.Y, _endPoint.X, _endPoint.Y);
                if (_startObject != null)
                {

                    _startPoint = _startObject.GetHandle(_startobjecthandle);
                }
                if (_endObject != null)
                {
                    _endPoint = _endObject.GetHandle(_endobjecthandle);
                }

                if (_startPoint.Y > _middlePoint.Y)
                    _middlePoint.Y = _startPoint.Y;
                if (_startPoint.X > _middlePoint.X)
                    _middlePoint.X = _startPoint.X;
                if (_endPoint.Y < _middlePoint.Y)
                    _middlePoint.Y = _endPoint.Y;
                if (_endPoint.X < _middlePoint.X)
                    _middlePoint.X = _endPoint.X;

                g.DrawLine(pen, _startPoint.X, _startPoint.Y, _middlePoint.X, _startPoint.Y);
                g.DrawLine(pen, _middlePoint.X, _startPoint.Y, _middlePoint.X, _endPoint.Y);
                g.DrawLine(pen, _middlePoint.X, _endPoint.Y, _endPoint.X, _endPoint.Y);

                pen.Dispose();
            }
            catch (Exception ex)
            {
                SVGErr.Log("DrawLine", "Draw", ex.ToString(), SVGErr._LogPriority.Info);
            }
        }

        public override PointF GetHandle(int handleNumber)
        {
            if (handleNumber == 1)
            {
                if (_startObject != null)
                {
                    _startobjecthandle = _startObject.HitAroundTest(_startPoint);
                    _startPoint = _startObject.GetHandle(_startobjecthandle);
                }
                return _startPoint;

            }

            if (handleNumber == 2)
            {
                return _middlePoint;

            }

            if (_endObject != null)
            {
                _endobjecthandle = _endObject.HitAroundTest(_endPoint);
                _endPoint = _endObject.GetHandle(_endobjecthandle);
            }

            return _endPoint;
        }

        /// <summary>
        /// 设置开始点或者结束点跟随的对象
        /// </summary>
        /// <param name="handleNumber"></param>
        public void SetFollowObject(int handleNumber,DrawObject drawObject)
        {
            if (handleNumber == 1)
            {
                _startObject = drawObject;
                _startobjecthandle = _startObject.HitAroundTest(_startPoint);
            }
            else if (handleNumber == 2)
            {

            }
            else
            {
                _endObject = drawObject;
                _endobjecthandle = _endObject.HitAroundTest(_endPoint);
            }
            
        }

        public void SetFollowObjectNull(int handleNumber)
        {
            if (handleNumber == 1)
                _startObject = null;
            else if (handleNumber != 2)
                _endObject = null;
        }

        ////TODO 等待段瑞重写
        //public override PointF GetKnobPoint()
        //{
        //    return new PointF((_startPoint.X + _endPoint.X) / 2, (_startPoint.Y + _endPoint.Y) / 2 - 20);
        //}
        public override Cursor GetHandleCursor(int handleNumber)
        {
            switch (handleNumber)
            {
                case 1:
                case 2:
                    return Cursors.SizeAll;
                default:
                    return Cursors.Default;
            }
        }

        public override string GetXmlStr(SizeF scale,bool noAnimation = true)
        {
            string s = "<";
            s += Tag;
            s += GetStrStyle(scale);
            float x1 = _startPoint.X / scale.Width;
            float y1 = _startPoint.Y / scale.Height;
            float x2 = _endPoint.X / scale.Width;
            float y2 = _endPoint.Y / scale.Height;
            float x3 = _middlePoint.X / scale.Width;
            float y3 = _middlePoint.Y / scale.Height;
            s += " x1 = \"" + x1.ToString(CultureInfo.InvariantCulture) + "\"";
            s += " y1 = \"" + y1.ToString(CultureInfo.InvariantCulture) + "\"";
            s += " x2 = \"" + x2.ToString(CultureInfo.InvariantCulture) + "\"";
            s += " y2 = \"" + y2.ToString(CultureInfo.InvariantCulture) + "\"";
            s += " x2 = \"" + x3.ToString(CultureInfo.InvariantCulture) + "\"";
            s += " y2 = \"" + y3.ToString(CultureInfo.InvariantCulture) + "\"";
            s += " />" + Environment.NewLine;
            return s;
        }

        /// <summary>
        /// Hit test.
        /// Return value: -1 - no hit
        ///                0 - hit anywhere
        ///                > 1 - handle number
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override int HitTest(PointF point)
        {
            if (Selected)
            {
                for (int i = 1; i <= HandleCount; i++)
                {
                    if (GetHandleRectangle(i).Contains(point))
                        return i;
                }
            }

            if (PointInObject(point))
                return 0;

            return -1;
        }

        public override bool IntersectsWith(RectangleF rectangle)
        {
            CreateObjects();

            return AreaRegion.IsVisible(rectangle);
        }

        public override void Move(float deltaX, float deltaY)
        {
            _startPoint.X += deltaX;
            _startPoint.Y += deltaY;
            _middlePoint.X += deltaX;
            _middlePoint.Y += deltaY;
            _endPoint.X += deltaX;
            _endPoint.Y += deltaY;

            Invalidate();
        }

        public override void MoveHandleTo(PointF point, int handleNumber)
        {


            if (handleNumber == 1)
            {
                _startPoint = point;

                if (_startPoint.Y > _middlePoint.Y)
                    _middlePoint.Y = _startPoint.Y;
                if (_startPoint.X > _middlePoint.X)
                    _middlePoint.X = _startPoint.X;


                if (_startPoint.Y > _middlePoint.Y)
                    _middlePoint.Y = _startPoint.Y;
                if (_startPoint.X > _middlePoint.X)
                    _middlePoint.X = _startPoint.X;
                if (_endPoint.Y < _middlePoint.Y)
                    _middlePoint.Y = _endPoint.Y;
                if (_endPoint.X < _middlePoint.X)
                    _middlePoint.X = _endPoint.X;

            }
            else if (handleNumber == 2)
            {
                _middlePoint = point;
                if (_startPoint.Y > _middlePoint.Y)
                    _middlePoint.Y = _startPoint.Y;
                if (_startPoint.X > _middlePoint.X)
                    _middlePoint.X = _startPoint.X;
                if (_endPoint.Y < _middlePoint.Y)
                    _middlePoint.Y = _endPoint.Y;
                if (_endPoint.X < _middlePoint.X)
                    _middlePoint.X = _endPoint.X;
            }
            else
            {
                _endPoint = point;
                {
                    if (_endPoint.Y < _middlePoint.Y)
                        _middlePoint.Y = _endPoint.Y;
                    if (_endPoint.X < _middlePoint.X)
                        _middlePoint.X = _endPoint.X;
                }

                if (_startPoint.Y > _middlePoint.Y)
                    _middlePoint.Y = _startPoint.Y;
                if (_startPoint.X > _middlePoint.X)
                    _middlePoint.X = _startPoint.X;
                if (_endPoint.Y < _middlePoint.Y)
                    _middlePoint.Y = _endPoint.Y;
                if (_endPoint.X < _middlePoint.X)
                    _middlePoint.X = _endPoint.X;
            }

            Invalidate();
        }

        public override void Resize(SizeF newscale, SizeF oldscale)
        {
            _startPoint = RecalcPoint(_startPoint, newscale, oldscale);
            _endPoint = RecalcPoint(_endPoint, newscale, oldscale);
            RecalcStrokeWidth(newscale, oldscale);
            Invalidate();
        }

        public override void SaveToXml(XmlTextWriter writer, double scale)
        {
            writer.WriteStartElement("line");
            string s = "stroke-width:" + StrokeWidth;
            writer.WriteAttributeString("style", s);
            writer.WriteAttributeString("x1", _startPoint.X.ToString());
            writer.WriteAttributeString("y1", _startPoint.Y.ToString());
            writer.WriteAttributeString("x2", _endPoint.X.ToString());
            writer.WriteAttributeString("y2", _endPoint.Y.ToString());
            writer.WriteEndElement();
        }

        /// <summary>
        /// Create graphic objects used from hit test.
        /// </summary>
        protected virtual void CreateObjects()
        {
            if (AreaPath != null)
                return;

            // Create path which contains wide line
            // for easy mouse selection
            AreaPath = new GraphicsPath();
            AreaPen = new Pen(Color.Black, 2);
            AreaPath.AddLine(_startPoint.X, _startPoint.Y, _endPoint.X, _endPoint.Y);
            if (_startPoint != _endPoint)
                AreaPath.Widen(AreaPen);

            // Create region from the path
            AreaRegion = new Region(AreaPath);
        }

        /// <summary>
        /// Invalidate object.
        /// When object is invalidated, path used for hit test
        /// is released and should be created again.
        /// </summary>
        protected void Invalidate()
        {
            if (AreaPath != null)
            {
                AreaPath.Dispose();
                AreaPath = null;
            }

            if (AreaPen != null)
            {
                AreaPen.Dispose();
                AreaPen = null;
            }

            if (AreaRegion != null)
            {
                AreaRegion.Dispose();
                AreaRegion = null;
            }
        }

        protected override bool PointInObject(PointF point)
        {
            CreateObjects();

            return AreaRegion.IsVisible(point);
        }

        #region 段瑞旋转

        protected override PointF RotatePoint(PointF center, PointF p1, float angle)
        {
            return base.RotatePoint(center, p1, angle);
        }

        public override void Rotate(float angle)
        {
            PointF center = new PointF((_startPoint.X + _endPoint.X) / 2, (_startPoint.Y + _endPoint.Y) / 2);

            _startPoint = RotatePoint(center, _startPoint, angle);
            _endPoint = RotatePoint(center, _endPoint, angle);

            Invalidate();
        }

        public override void Rotate(float angle, PointF center)
        {
            _startPoint = RotatePoint(center, _startPoint, angle);
            _endPoint = RotatePoint(center, _endPoint, angle);

            Invalidate();
        }

        public override void Rotate(float angle, float x, float y)
        {
            PointF center = new PointF(x, y);

            _startPoint = RotatePoint(center, _startPoint, angle);
            _endPoint = RotatePoint(center, _endPoint, angle);

            Invalidate();
        }

        #endregion

        #endregion 函数
    }
}
