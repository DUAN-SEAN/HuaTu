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
        #endregion 字段

        #region 构造器
        public SingleDisConnectorDrawObject()
        {
            SetRectangleF(0, 0, 1, 1);
            Switch = 1;
            Initialize();//设置缩放比例
        }

        public SingleDisConnectorDrawObject(float x, float y, float width, float height,int switchstate)
        {
            RectangleF = new RectangleF(x, y, width, height);
            Switch = switchstate;
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
            PointF top = new PointF(r.X + r.Width / 2, r.Y);
            PointF topmid = new PointF(r.X + r.Width / 2, r.Y + 横线);
            g.DrawLine(pen, top, topmid);

            //中间横线
            PointF topleft = new PointF(r.X + r.Width / 2 - 横线 / 2, r.Y + 横线);
            PointF topright = new PointF(r.X + r.Width / 2 + 横线 / 2, r.Y + 横线);
            g.DrawLine(pen, topleft, topright);


            //最后一根竖线
            PointF bottom = new PointF(r.X + r.Width / 2, r.Y + r.Height);
            PointF bottommid = new PointF(r.X + r.Width / 2, r.Y + r.Height - 横线);
            g.DrawLine(pen, bottom, bottommid);

            PointF Leftpoint = new PointF(r.X, r.Y + 横线);
            //PointF RightPoint = new PointF(r.X + r.Width, r.Y + 横线);
            if (Switch == 0)
            {
                g.DrawLine(pen, bottommid, topmid);
            }
            else
            {
                g.DrawLine(pen, bottommid, Leftpoint);
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


    }
}
