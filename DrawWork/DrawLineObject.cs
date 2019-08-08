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
    public class DrawLineObject : DrawObject
    {
        #region 字段

        private const string Tag = "line";

        private PointF _endPoint;
        private PointF _startPoint;

        #endregion 字段

        #region 构造器

        public DrawLineObject()
        {
            _startPoint.X = 0;
            _startPoint.Y = 0;
            _endPoint.X = 1;
            _endPoint.Y = 1;

            Initialize();
        }

        public DrawLineObject(float x1, float y1, float x2, float y2)
        {
            _startPoint.X = x1;
            _startPoint.Y = y1;
            _endPoint.X = x2;
            _endPoint.Y = y2;

            Initialize();
        }

        #endregion 构造器

        #region 属性

        public override int HandleCount
        {
            get
            {
                return 2;
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

        public override void Draw(Graphics g)
        {
            try
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                var pen = new Pen(Stroke, StrokeWidth);
                PointF centerTemp = GetCenter();
                if (Parent != null)//表示有设备父实体 则要求应用
                {
                    var worldObj =  GetWorldDrawObject();
                    var drawPoint1 = worldObj._startPoint;
                    var drawPoint2 = worldObj._endPoint;
                    //最终绘画
                    g.DrawLine(pen, drawPoint1.X, drawPoint1.Y, drawPoint2.X, drawPoint2.Y);
                    pen.Dispose();
                }
                else
                {
                    var drawPoint1 = RotatePoint(centerTemp,
                        new PointF(_startPoint.X + centerTemp.X, _startPoint.Y + centerTemp.Y), _angle);
                    var drawPoint2 = RotatePoint(centerTemp, new PointF(_endPoint.X + centerTemp.X, _endPoint.Y + centerTemp.Y), _angle);
                    ;
                    g.DrawLine(pen, drawPoint1.X, drawPoint1.Y, drawPoint2.X, drawPoint2.Y);
                    pen.Dispose();
                }

                
            }
            catch (Exception ex)
            {
                SVGErr.Log("DrawLine", "Draw", ex.ToString(), SVGErr._LogPriority.Info);
            }
        }
        
        public override PointF GetCenter()
        {
            return new PointF((_startPoint.X + _endPoint.X) / 2, (_startPoint.Y + _endPoint.Y) / 2);
        }

        public override PointF GetHandle(int handleNumber)
        {
            PointF centerTemp = GetCenter();
            if (Parent != null)
            {
                centerTemp = Parent.GetCenter();
            }

            var drawPoint1 = RotatePoint(centerTemp, _startPoint, _angle);
            var drawPoint2 = RotatePoint(centerTemp, _endPoint, _angle);

            if (handleNumber == 1)
                return drawPoint1;

            return drawPoint2;
        }
        //TODO 等待段瑞重写
        public override PointF GetKnobPoint()
        {
            return new PointF((_startPoint.X + _endPoint.X)/2, (_startPoint.Y + _endPoint.Y) / 2 - 20);
        }
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

        public override string GetXmlStr(SizeF scale,bool noAnimation= true)
        {
            string s = "<";
            s += Tag;
            s += GetStrStyle(scale);
            float x1 = _startPoint.X / scale.Width;
            float y1 = _startPoint.Y / scale.Height;
            float x2 = _endPoint.X / scale.Width;
            float y2 = _endPoint.Y / scale.Height;

            s += " x1 = \"" + x1.ToString(CultureInfo.InvariantCulture) + "\"";
            s += " y1 = \"" + y1.ToString(CultureInfo.InvariantCulture) + "\"";
            s += " x2 = \"" + x2.ToString(CultureInfo.InvariantCulture) + "\"";
            s += " y2 = \"" + y2.ToString(CultureInfo.InvariantCulture) + "\"";
            s += noAnimation ? " />" : " >";
            s += "\r\n";
            return s;
        }

        public override string GetXmlEnd()
        {
            return "</line>"+base.GetXmlEnd();
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

            _endPoint.X += deltaX;
            _endPoint.Y += deltaY;

            Invalidate();
        }

        
        public override void MoveHandleTo(PointF point, int handleNumber)
        {
            if (handleNumber == 1)
                _startPoint = point;
            else
                _endPoint = point;

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

        public override Cursor GetOutlineCursor(int handleNumber)
        {
            return base.GetOutlineCursor(handleNumber);
        }

        protected override bool PointInObject(PointF point)
        {
            CreateObjects();

            return AreaRegion.IsVisible(point);
        }
        /// <summary>
        /// 获取该物体在世界坐标下的变换物体
        /// </summary>
        /// <returns></returns>
        public new  DrawLineObject  GetWorldDrawObject()
        {
            PointF parents = new PointF(Parent.Rectangle.X, Parent.Rectangle.Y);
            DeviceDrawObjectBase par = Parent;
            
            while (par.Parent != null)
            {
                par = par.Parent;
                parents.X += par.Rectangle.X;
                parents.Y += par.Rectangle.Y;
            }

            if (Parent != null)//表示有设备父实体 则要求应用
            {
                ////获取父物体的世界坐标
                //var parentPosition = new PointF(Parent.Rectangle.X, Parent.Rectangle.Y);
                ////应用缩放 获取缩放比
                //var zoomw = Parent.Width / Parent.ViewBox_w;
                //var zoomh = Parent.Height / Parent.ViewBox_h;

                //吴悠修改
                //获取父物体的世界坐标
                var parentPosition = new PointF(parents.X, parents.Y);
                //应用缩放 获取缩放比
                var zoomw = par.Width / par.ViewBox_w;
                var zoomh = par.Height / par.ViewBox_h;

                // 固定左上角的点缩放
                var zoomStart = new PointF(_startPoint.X*zoomw, _startPoint.Y * zoomh);
                var zoomEnd = new PointF(_endPoint.X*zoomw, _endPoint.Y * zoomh);

                //移动到世界坐标
                var moveStart = new PointF(parentPosition.X + zoomStart.X, parentPosition.Y + zoomStart.Y);
                var moveEnd = new PointF(parentPosition.X + zoomEnd.X, parentPosition.Y + zoomEnd.Y);

                //最终旋转 获取父物体的位置
                var centerTemp = Parent.GetCenter();
                var drawPoint1 = RotatePoint(centerTemp, moveStart, _angle);
                var drawPoint2 = RotatePoint(centerTemp, moveEnd, _angle);

                return new DrawLineObject(drawPoint1.X, drawPoint1.Y, drawPoint2.X, drawPoint2.Y);
            }

            return this;
        }

        #region 段瑞旋转

        protected override PointF RotatePoint(PointF center, PointF p1, float angle)
        {
            return base.RotatePoint(center, p1, angle);
        }

        public override void Rotate(float angle)
        {
            //PointF center = new PointF((_startPoint.X + _endPoint.X) / 2, (_startPoint.Y + _endPoint.Y) / 2);

            //_startPoint = RotatePoint(center, _startPoint, angle);
            //_endPoint = RotatePoint(center, _endPoint, angle);
            _angle -= angle;
            Invalidate();
        }

        public override void Rotate(float angle, PointF center)
        {
            //_startPoint = RotatePoint(center, _startPoint, angle);
            //_endPoint = RotatePoint(center, _endPoint, angle);

            _angle -= angle;

            Invalidate();
        }

        public override void Rotate(float angle, float x, float y)
        {
            //PointF center = new PointF(x, y);

            //_startPoint = RotatePoint(center, _startPoint, angle);
            //_endPoint = RotatePoint(center, _endPoint, angle);
            _angle -= angle;
            Invalidate();
        }

        #endregion

        #endregion 函数
    }
}
