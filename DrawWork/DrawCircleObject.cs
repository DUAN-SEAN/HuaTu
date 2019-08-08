using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SVGHelper;

namespace DrawWork
{
    public class DrawCircleObject:DrawRectangleObject
    {
        private const string Tag = "circle";

        public float CX
        {
            set { }
            get
            {
                fixedCenter = GetCenter();
                return fixedCenter.X;
            }
        }

        public float CY
        {
            set { }
            get
            {
                fixedCenter = GetCenter();
                return fixedCenter.Y;
            }
        }

        public float R
        {
            get { return Height < Width ? Height/2 : Width/2; }
        }

        public DrawCircleObject()
        {
        }

        public DrawCircleObject(float x, float y, float w, float h) : base(x, y, w, h)
        {

        }
        public override void Draw(Graphics g)
        {
            
            

             if (hasRotation)
             {
                fixedCenter = GetCenter();
                hasRotation = false;
             }
             PointF center = new PointF(fixedCenter.X + parentPointF.X, fixedCenter.Y + parentPointF.Y);

             RectangleF r = GetNormalizedRectangle(ParentAndRectangleF);
             if (Parent != null)
             {
                center = Parent.GetRoot().GetCenter();
                var worldDrawObj = GetWorldDrawObject();
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

        public new DrawCircleObject GetWorldDrawObject()
        {

            if (Parent != null)
            {
                
              
                PointF worldTemp = new PointF();
                var tempR = new PointF(rectangle.Width,rectangle.Height);
                worldTemp.X = rectangle.X;//当前坐标
                worldTemp.Y = rectangle.Y;
                var worldPosition= new PointF(worldTemp.X,worldTemp.Y);
                var p = Parent;
                while (p != null)
                {

                    if (p.Width == 0 || p.Height == 0)
                    {
                        //worldTemp.X += p.Rectangle.X;
                        //worldTemp.Y += p.Rectangle.Y;
                    }
                    else
                    {
                        var zw = p.Width / p.ViewBox_w;
                        var zh = p.Height / p.ViewBox_h;
                        worldPosition.X += worldTemp.X*zw;
                        worldPosition.Y += worldTemp.Y*zh;
                        tempR.X += tempR.X * zw;
                        tempR.Y += tempR.Y * zh;
                    }

                    worldTemp.X = p.Rectangle.X;
                    worldTemp.Y = p.Rectangle.Y;


                    p = p.Parent;
                }

                worldPosition.X += worldTemp.X;
                worldPosition.Y += worldTemp.Y;

                //var worldDrawObj = new DrawCircleObject(worldPosition.X, worldPosition.Y, tempR.X, tempR.Y);
                var worldDrawObj = new DrawCircleObject(worldPosition.X, worldPosition.Y, 5, 5);
                
                return worldDrawObj;
            }

            return this;
        }

        public override void Update()
        {
            base.Update();
            //RectangleF r = GetNormalizedRectangle(RectangleF);
            //if (r.Height < r.Width)
            //{
            //    rectangle.Width = rectangle.Height;
            //}
            //else
            //{
            //    rectangle.Height = rectangle.Width;
            //}

        }

        public override void MoveHandleTo(PointF point, int handleNumber)
        {
            float left = rectangle.Left;
            float top = rectangle.Top;
            float right = rectangle.Right;
            float bottom = rectangle.Bottom;

            float x, xCenter, yCenter;

            xCenter = rectangle.X + rectangle.Width / 2;
            yCenter = rectangle.Y + rectangle.Height / 2;
            x = rectangle.X;
            float y = rectangle.Y;

            //PointF center = new PointF(xCenter, yCenter);
            PointF center = fixedCenter;

            PointF temp = default(PointF);

            PointF toRectangleMousePoint = default;//将鼠标位置转换到矩形坐标系

            switch (handleNumber)
            {
                case 1: //左上
                    toRectangleMousePoint = RotatePointReverse(center, point, _angle);

                    left = toRectangleMousePoint.X;
                    top = toRectangleMousePoint.Y;
                    break;
                case 2: //上
                    x = xCenter;
                    y = rectangle.Y;
                    temp = new PointF(x, y);
                    toRectangleMousePoint = GetLocalMousePoint(point, center, temp);
                    top = toRectangleMousePoint.Y;
                    
                    break;
                case 3: //右上
                    toRectangleMousePoint = RotatePointReverse(center, point, _angle);
                    right = toRectangleMousePoint.X;
                    top = toRectangleMousePoint.Y;
                    break;
                case 4: //右
                    x = right;
                    y = yCenter;
                    temp = new PointF(x, y);
                    toRectangleMousePoint = GetLocalMousePoint(point, center, temp);
                    right = toRectangleMousePoint.X;
                    break;
                case 5: //右下
                    toRectangleMousePoint = RotatePointReverse(center, point, _angle);
                    right = toRectangleMousePoint.X;
                    bottom = toRectangleMousePoint.Y;
                    break;
                case 6: //下
                    x = xCenter;
                    y = bottom;
                    temp = new PointF(x, y);
                    toRectangleMousePoint = GetLocalMousePoint(point, center, temp);
                    bottom = toRectangleMousePoint.Y;
                    break;
                case 7:  //左下
                    toRectangleMousePoint = RotatePointReverse(center, point, _angle);
                    left = toRectangleMousePoint.X;
                    bottom = toRectangleMousePoint.Y;
                    break;
                case 8:  //左
                    x = left;
                    y = yCenter;
                    temp = new PointF(x, y);
                    toRectangleMousePoint = GetLocalMousePoint(point, center, temp);
                    left = toRectangleMousePoint.X;
                    break;
            }

            SetRectangleF(left, top, right - left, bottom - top);
        }

        public override string GetXmlStr(SizeF scale, bool noAnimation = true)
        {
            string s = "<";
            s += Tag;
            s += GetStrStyle(scale);
            s += " cx = \"" + CX + "\"";
            s += " cy = \"" + CY + "\"";
            s += " r = \"" + R + "\"";
            s += GetTransformXML(_angle, fixedCenter);


           
           

            s += noAnimation ? " />" : " >";
            s += "\r\n";
            return s;
        }

        public static DrawCircleObject Create(SVGCircle svg)
        {
            float x = float.Parse(svg.CX) - float.Parse(svg.R);
            float y = float.Parse(svg.CY) - float.Parse(svg.R);
            float wh = float.Parse(svg.R) * 2;

            DrawCircleObject o = new DrawCircleObject(x,y,wh,wh);

            return o;
        }
        public override string GetXmlEnd()
        {
            return "</circle>" + base.GetXmlEnd();
        }
    }
}
