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

        /// <summary>
        /// 设备实体Id
        /// </summary>
        public string _EntityId;
        public DeviceDrawObjectBase()
        {
            SetRectangleF(0, 0, 1, 1);
            Initialize();
        }

        public DeviceDrawObjectBase(float x, float y, float width, float height,string entityId,List<DrawObject> drawobjs,List<DeviceDrawObjectBase> deviceDrawObjectBases)
        {
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;
            _EntityId = entityId;
            drawObjects = drawobjs;
            this.deviceDrawObjectBases = deviceDrawObjectBases;
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

        /// <summary>
        /// 获取设备实体的xm
        /// 1 获取内部引用的symbolUnit，检测是否与定义的相同，如果不相同则生成新的unit 以及新的设备类型
        /// 2 获取内部元数据，定义metaData
        /// </summary>
        /// <param name="scale"></param>
        /// <param name="noAnimation"></param>
        /// <returns></returns>
        public override string GetXmlStr(SizeF scale, bool noAnimation = true)
        {
            string s = "";
            return s;

        }
    }
}
