﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawWork.Animation;
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





            foreach (var animationBase in AnimationBases)
            {
                if (animationBase.AnimationType == AnimationType.AnimationPath)
                    if (animationBase is AnimationPath path)
                    {
                        if (!int.TryParse(path.TimingAttr.Dur, out int dur)) continue;
                        if (path._worldpath.Length == 0) continue;

                        int index = lastdrawtime < 0.05
                            ? 0
                            : (int) ((lastdrawtime / dur /
                                      ((dur / (float) (path._worldpath.Length - 1)) / dur)) + 1);
                        if (index >= path._worldpath.Length)
                        {
                            lastdrawtime = (lastdrawtime + SVGDefine.AnimationSpeed) % dur;
                            continue;
                        }
                        var point = path._worldpath[index];
                        if (index == 0)
                        {
                            rectangle.X = point.X;
                            rectangle.Y = point.Y;
                        }
                        else
                        {
                            var lastpoint = path._worldpath[index - 1];
                            float proc = ((float) lastdrawtime) / (float) dur ;
                            rectangle.X = lastpoint.X + ( point.X - lastpoint.X) * proc;
                            rectangle.Y = lastpoint.Y + ( point.Y - lastpoint.Y) * proc;

                        }

                        lastdrawtime = (lastdrawtime + SVGDefine.AnimationSpeed ) % dur;
                        break;
                    }
            }

            PointF center = new PointF(fixedCenter.X + parentPointF.X, fixedCenter.Y + parentPointF.Y);

             RectangleF r = GetNormalizedRectangle(ParentAndRectangleF);
             if (Parent != null)
             {
                 center = Parent.GetRoot().GetCenter();
                 var worldDrawObj = GetWorldDrawObject();
                 r = GetNormalizedRectangle(worldDrawObj.rectangle);
             }
             if (ParentDrawObject != null)
             {
                 center = ParentDrawObject.GetRoot().GetCenter();
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
                var tempR = new PointF(rectangle.Width, rectangle.Height);
                worldTemp.X = rectangle.X;//当前坐标
                worldTemp.Y = rectangle.Y;
                var worldPosition = new PointF(0f, 0f);
                var worldR = PointF.Empty;
                var p = Parent;
                while (p != null)
                {
                    var zw = 0f;
                    var zh = 0f;
                    if (p.Width == 0 || p.Height == 0)
                    {
                        zw = 1f;
                        zh = 1f;
                        //worldTemp.X += p.Rectangle.X;
                        //worldTemp.Y += p.Rectangle.Y;
                    }
                    else
                    {
                        zw = p.Width / p.ViewBox_w;
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
            if (AnimationBases.Count == 0)
            {
               
                s += Tag;
                s += GetStrStyle(scale);
                s += " cx = \"" + CX + "\"";
                s += " cy = \"" + CY + "\"";
                s += " r = \"" + R + "\"";
                s += GetTransformXML(_angle, fixedCenter);
            }
            else
            {
                for (int i = 0; i < AnimationBases.Count; i++)
                {
                    if (AnimationBases[i] is AnimationPath path)
                    {
                        s += Tag;
                        s += GetStrStyle(scale);
                        s += " cx = \"" + path._worldpath[0].X + "\"";
                        s += " cy = \"" + path._worldpath[0].Y + "\"";
                        s += " r = \"" + R + "\"";
                        s += GetTransformXML(_angle, fixedCenter);
                        break;
                    }
                }
            }


            
           

            s += noAnimation ? " />" : " >";
            s += Environment.NewLine;
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
            return "</circle>";
        }
    }
}
