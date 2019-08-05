using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork
{
    public class DeviceDrawObjectBase : DrawRectangleObject
    {
        /// <summary>
        /// 设备引用集合
        /// 可能只是用ID描述
        /// </summary>
        public List<DeviceDrawObjectBase> deviceDrawObjectBases;

        /// <summary>
        /// 非引用的绘画本体图元
        /// </summary>
        public List<DrawObject> drawObjects;

        /// <summary>
        /// 数据集合
        /// </summary>
        public List<Object> models;

        public DeviceDrawObjectBase()
        {
            SetRectangleF(0, 0, 1, 1);
            Initialize();
        }

        public DeviceDrawObjectBase(float x, float y, float width, float height)
        {
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;
            Initialize();
        }
        /// <summary>
        /// 获取当前中心点
        /// </summary>
        /// <returns></returns>
        protected virtual PointF GetCenter()
        {
            float x, xCenter, yCenter;

            xCenter = rectangle.X + rectangle.Width / 2;
            yCenter = rectangle.Y + rectangle.Height / 2;
            return new PointF(xCenter, yCenter);
        }


        public override void Update()
        {
            for (int i = 0; i < drawObjects.Count; i++)
            {
                drawObjects[i].Update();
            }

            for (int i = 0; i < deviceDrawObjectBases.Count; i++)
            {
                deviceDrawObjectBases[i].Update();
            }

        }

        public override void Draw(Graphics g)
        {
            for (int i = 0; i < drawObjects.Count; i++)
            {
                drawObjects[i].Draw(g);
            }

            for (int i = 0; i < deviceDrawObjectBases.Count; i++)
            {
                deviceDrawObjectBases[i].Draw(g);
            }

            


        }
    }
}
