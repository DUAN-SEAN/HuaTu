using SVGHelper;
using SVGHelper.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork
{   
    /// <summary>
    /// 继承自矩形，实际上只是显示效果与矩形不同以外 其他的和矩形一样
    /// </summary>
    public class DrawEllipseObject : DrawRectangleObject
    {
        #region 字段

        private const string Tag = "ellipse";

        #endregion 字段

        #region 构造器

        public DrawEllipseObject()
        {
            SetRectangleF(0, 0, 1, 1);
            Initialize();//设置缩放比例
        }

        public DrawEllipseObject(float x, float y, float width, float height)
        {
            RectangleF = new RectangleF(x, y, width, height);
            Initialize();
        }

        #endregion 构造器

        #region 函数

        public static DrawEllipseObject Create(SVGEllipse svg)
        {
            try
            {
                float cx = ParseSize(svg.CX, Dpi.X);
                float cy = ParseSize(svg.CY, Dpi.Y);
                float rx = ParseSize(svg.RX, Dpi.X);
                float ry = ParseSize(svg.RY, Dpi.Y);
                var dobj = new DrawEllipseObject(cx - rx, cy - ry, rx * 2, ry * 2);
                dobj.SetStyleFromSvg(svg);
                return dobj;
            }
            catch (Exception ex)
            {
                SVGErr.Log("DrawEllipse", "CreateRectangle", ex.ToString(), SVGErr._LogPriority.Info);
                return null;
            }
        }

        public new  DrawEllipseObject GetWorldDrawObject()
        {
            if(Parent != null)
            {


                PointF worldTemp = new PointF();
                var tempR = new PointF(rectangle.Width, rectangle.Height);
                worldTemp.X = rectangle.X;//当前坐标
                worldTemp.Y = rectangle.Y;
                var worldPosition = new PointF(0f, 0f);
                var worldR = PointF.Empty;
                var p = Parent;
                while (p != null)
                {
                    var zw = 0f;
                    var zh =0f;
                    if (p.Width == 0 || p.Height == 0)
                    {
                        zw = 1f;
                        zh = 1f;
                        //worldTemp.X += p.Rectangle.X;
                        //worldTemp.Y += p.Rectangle.Y;
                    }
                    else
                    {   zw = p.Width / p.ViewBox_w;
                        zh = p.Height / p.ViewBox_h;
                   
                        
                    }
                    worldPosition.X += worldTemp.X * zw;
                    worldPosition.Y += worldTemp.Y * zh;
                    worldR.X += tempR.X * zw;
                    worldR.Y += tempR.Y * zh;

                    worldTemp.X = p.Rectangle.X;
                    worldTemp.Y = p.Rectangle.Y;

                    tempR = worldR;

                    p = p.Parent;
                }

                worldPosition.X += worldTemp.X;
                worldPosition.Y += worldTemp.Y;

                var worldDrawObj = new DrawEllipseObject(worldPosition.X, worldPosition.Y, worldR.X, worldR.Y);
                //var worldDrawObj = new DrawEllipseObject(wokrldPosition.X, worldPosition.Y, 5, 5);

                return worldDrawObj;
            }

            return this;
        }

        public override void Draw(Graphics g)
        {
            if (hasRotation)
            {
                fixedCenter = GetCenter();
                hasRotation = false;
            }
            PointF center = fixedCenter;

            RectangleF r = GetNormalizedRectangle(RectangleF);

            if (Parent != null)
            {
                center = Parent.GetCenter();
                var worldDrawObj =  GetWorldDrawObject();
                r = GetNormalizedRectangle(worldDrawObj.rectangle);
            }


            g.TranslateTransform(center.X, center.Y);
            g.RotateTransform(-_angle);
            g.TranslateTransform(-center.X, -center.Y);
            



            if (Fill != Color.Empty)
            {
                Brush brush = new SolidBrush(Fill);
                g.FillEllipse(brush, r);
            }
            var pen = new Pen(Stroke, StrokeWidth);
            g.DrawEllipse(pen, r);
            g.ResetTransform();
            pen.Dispose();
        }

        public override string GetXmlStr(SizeF scale,bool noAnimation = true)
        {
            string s = "<";
            s += Tag;
            s += GetStrStyle(scale);
            float cx = (RectangleF.X + RectangleF.Width / 2) / scale.Width;
            float cy = (RectangleF.Y + RectangleF.Height / 2) / scale.Height;
            float rx = (RectangleF.Width / 2) / scale.Width;
            float ry = ((RectangleF.Height / 2)) / scale.Height;

            s += " cx = \"" + cx.ToString(CultureInfo.InvariantCulture) + "\"";
            s += " cy = \"" + cy.ToString(CultureInfo.InvariantCulture) + "\"";
            s += " rx = \"" + rx.ToString(CultureInfo.InvariantCulture) + "\"";
            s += " ry = \"" + ry.ToString(CultureInfo.InvariantCulture) + "\"";
            s += GetTransform(_angle, fixedCenter);
            s += noAnimation ? " />" : " >";
            s += Environment.NewLine;
          
            return s;
        }
        public static string GetTransform(float angle, PointF center)
        {
            return $" transform=\"rotate({-angle}, {center.X} {center.Y})\"";
        }

     
        #endregion 函数


    }
}
