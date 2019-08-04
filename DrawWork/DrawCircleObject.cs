using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork
{
    public class DrawCircleObject:DrawRectangleObject
    {
        private const string Tag = "circle";

        public float CX
        {
            set { }
            get { return fixedCenter.X; }
        }

        public float CY
        {
            set { }
            get { return fixedCenter.Y; }
        }

        public float R
        {
            get { return Height < Width ? Height/2 : Width/2; }
        }

        public override void Draw(Graphics g)
        {

             if (hasRotation)
            {
                fixedCenter = GetCenter();
                hasRotation = false;
            }
            PointF center = fixedCenter;
            g.TranslateTransform(center.X, center.Y);
            g.RotateTransform(-_angle);
            g.TranslateTransform(-center.X, -center.Y);
        


            RectangleF r = GetNormalizedRectangle(RectangleF);

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


        public override void Update()
        {
            base.Update();
            RectangleF r = GetNormalizedRectangle(RectangleF);
            if (r.Height > r.Width)
            {
                r.Width = r.Height;
            }
            else
            {
                r.Height = r.Width;
            }
            
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

        public override string GetXmlEnd()
        {
            return "</circle>" + base.GetXmlEnd();
        }
    }
}
