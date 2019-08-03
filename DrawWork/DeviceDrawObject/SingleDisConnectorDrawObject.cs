using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork
{
    public class SingleDisConnectorDrawObject : DeviceDrawObject
    {
        #region 字段
        private const string Tag = "DisConnector:刀闸@0";
      
        private DeviceDrawObject device1;

        //第一根竖线
        PointF top;
        PointF topmid;

        //中间横线
        PointF topleft;
        PointF topright;

        //最后一根竖线
        PointF bottom;
        PointF bottommid;

        PointF Leftpoint;
        #endregion 字段

        #region 构造器
        public SingleDisConnectorDrawObject()
        {
            drawObjects = new List<DrawObject>();

            SetRectangleF(0, 0, 1, 1);
            Switch = 1;
            RectangleF r = GetNormalizedRectangle(RectangleF);

            float 横线 = r.Height * 0.2f;
            //第一根竖线
            top = new PointF(r.X + r.Width / 2, r.Y);
            topmid = new PointF(r.X + r.Width / 2, r.Y + 横线);

            //中间横线
            topleft = new PointF(r.X + r.Width / 2 - 横线 / 2, r.Y + 横线);
            topright = new PointF(r.X + r.Width / 2 + 横线 / 2, r.Y + 横线);


            //最后一根竖线
            bottom = new PointF(r.X + r.Width / 2, r.Y + r.Height);
            bottommid = new PointF(r.X + r.Width / 2, r.Y + r.Height - 横线);

            Leftpoint = new PointF(r.X, r.Y + 横线);
            drawObjects.Add(new DrawLineObject(top.X, top.Y, topmid.X, topmid.Y));
            drawObjects.Add(new DrawLineObject(bottom.X, bottom.Y, bottommid.X, bottommid.Y));
            if (Switch == 0)
            {
                drawObjects.Add(new DrawLineObject(bottommid.X, bottommid.Y, topmid.X, topmid.Y));
            }
            else
            {
                drawObjects.Add(new DrawLineObject(bottommid.X, bottommid.Y, Leftpoint.X, Leftpoint.Y));
            }

            Initialize();//设置缩放比例
        }

        public SingleDisConnectorDrawObject(float x, float y, float width, float height,int switchstate)
        {

            drawObjects = new List<DrawObject>();
            RectangleF = new RectangleF(x, y, width, height);
            Switch = switchstate;
            RectangleF r = GetNormalizedRectangle(RectangleF);

            float 横线 = r.Height * 0.2f;
            //第一根竖线
            top = new PointF(r.X + r.Width / 2, r.Y);
            topmid = new PointF(r.X + r.Width / 2, r.Y + 横线);

            //中间横线
            topleft = new PointF(r.X + r.Width / 2 - 横线 / 2, r.Y + 横线);
            topright = new PointF(r.X + r.Width / 2 + 横线 / 2, r.Y + 横线);


            //最后一根竖线
            bottom = new PointF(r.X + r.Width / 2, r.Y + r.Height);
            bottommid = new PointF(r.X + r.Width / 2, r.Y + r.Height - 横线);

            Leftpoint = new PointF(r.X, r.Y + 横线);

            drawObjects.Add(new DrawLineObject(top.X,top.Y,topmid.X,topmid.Y));
            drawObjects.Add(new DrawLineObject(topleft.X, topleft.Y, topright.X, topright.Y));
            drawObjects.Add(new DrawLineObject(bottom.X, bottom.Y, bottommid.X, bottommid.Y));
            var animation = new Animation.Animation();

            if (Switch == 0)
            {
                drawObjects.Add(new DrawLineObject(bottommid.X, bottommid.Y, topmid.X, topmid.Y));
            }
            else
            {
                drawObjects.Add(new DrawLineObject(bottommid.X, bottommid.Y, Leftpoint.X, Leftpoint.Y));
            }
            Initialize();
        }
        #endregion 构造器


        #region 属性
        public override int HandleCount
        {
            get
            {
                return 9;
            }
        }

        /// <summary>
        /// 0 闭上
        /// 1 打开
        /// </summary>
        public int Switch
        {
            set;
            get;
        }

        #endregion 属性
        #region 函数
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
           
            var pen = new Pen(Stroke, StrokeWidth);
            float 横线 = r.Height * 0.2f;
            //第一根竖线
            top = new PointF(r.X + r.Width / 2, r.Y);
            topmid = new PointF(r.X + r.Width / 2, r.Y + 横线);
            drawObjects[0] = new DrawLineObject(top.X, top.Y, topmid.X, topmid.Y);
            

            //中间横线
            topleft = new PointF(r.X + r.Width / 2 - 横线 / 2, r.Y + 横线);
            topright = new PointF(r.X + r.Width / 2 + 横线 / 2, r.Y + 横线);
            drawObjects[1] = new DrawLineObject(topleft.X, topleft.Y, topright.X, topright.Y);

            //最后一根竖线
            bottom = new PointF(r.X + r.Width / 2, r.Y + r.Height);
            bottommid = new PointF(r.X + r.Width / 2, r.Y + r.Height - 横线);
            drawObjects[2] = new DrawLineObject(bottom.X, bottom.Y, bottommid.X, bottommid.Y);



            //var anim = new Animation.Animation();
            //anim.AnimationAttr.AttributeName = "x1";
            //anim.TimingAttr.RepeatCount = 5;
            //anim.TimingAttr.Dur = 20.ToString();
            //anim.From = 0.ToString();
            //anim.To = 50.ToString();

            //SetAnimation(drawObjects[2].Id.ToString(),anim);



            //drawObjects[2].AnimationBases.Add(anim);



            Leftpoint = new PointF(r.X, r.Y + 横线);
            //PointF RightPoint = new PointF(r.X + r.Width, r.Y + 横线);
            if (Switch == 0)
            {
                drawObjects[3] = new DrawLineObject(bottommid.X, bottommid.Y, topmid.X, topmid.Y);
            }
            else
            {
                drawObjects[3] = new DrawLineObject(bottommid.X, bottommid.Y, Leftpoint.X, Leftpoint.Y);
            }

            foreach (var VARIABLE in drawObjects)
            {
                VARIABLE.Draw(g);
            }


            g.ResetTransform();
            pen.Dispose();
        }

        public override PointF GetHandle(int handleNumber)
        {
            switch (handleNumber)
            {
                case 9:
                    return GetCenter();
              
            }
            return base.GetHandle(handleNumber);
        }


        public override void Update()
        {
            _devicestate = Switch == 0 ? 1 : 0;

            foreach (var handledeviceValue in handledevice.Values)
            {
                foreach (var VARIABLE in handledeviceValue)
                {
                    VARIABLE.DeviceState = DeviceState;
                }
            }


        }

        #endregion 函数

      
        //获取设备的旋转
        public override string GetTransformXML(float angle, PointF center)
        {
            return $" transform=\"rotate({-angle}, {center.X} {center.Y})\"";
        }
    }
}
