using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawWork.Symbol;

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

        /// <summary>
        /// 引用的设备id
        /// </summary>
        public string _hrefId;//引用设备id
        public DeviceDrawObjectBase()
        {
            SetRectangleF(0, 0, 1, 1);
            Initialize();
        }

        public DeviceDrawObjectBase(float x, float y, float width, float height,string entityId,List<DrawObject> drawobjs,List<DeviceDrawObjectBase> deviceDrawObjectBases,string hrefId)
        {
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;
            _EntityId = entityId;
            drawObjects = drawobjs;
            this.deviceDrawObjectBases = deviceDrawObjectBases;
            this._hrefId = hrefId;
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
            if (drawObjects != null)
            {
                for (int i = 0; i < drawObjects.Count; i++)
                {
                    drawObjects[i].Update();
                }
            }


            if (deviceDrawObjectBases != null)
            {

                for (int i = 0; i < deviceDrawObjectBases.Count; i++)
                {
                    deviceDrawObjectBases[i].Update();
                }
            }

        }

        public override void Rotate(float angle)
        {
            base.Rotate(angle);
            if (drawObjects != null)
            {
                for (int i = 0; i < drawObjects.Count; i++)
                {
                    drawObjects[i].Rotate(angle);
                }
            }


            if (deviceDrawObjectBases != null)
            {

                for (int i = 0; i < deviceDrawObjectBases.Count; i++)
                {
                    deviceDrawObjectBases[i].Rotate(angle);
                }
            }

        }

        public override void Draw(Graphics g)
        {
            if(drawObjects!=null)
                for (int i = 0; i < drawObjects.Count; i++)
                {
                    drawObjects[i].Draw(g);
                }
            if(deviceDrawObjectBases!=null)
                for (int i = 0; i < deviceDrawObjectBases.Count; i++)
                {
                    deviceDrawObjectBases[i].Draw(g);
                }
        }

        public override void Resize(SizeF newscale, SizeF oldscale)
        {
            base.Resize(newscale, oldscale);

            if (drawObjects != null)
                for (int i = 0; i < drawObjects.Count; i++)
                {
                    drawObjects[i].Resize(newscale, oldscale);
                }
            if (deviceDrawObjectBases != null)
                for (int i = 0; i < deviceDrawObjectBases.Count; i++)
                {
                    deviceDrawObjectBases[i].Resize(newscale, oldscale);
                }
        }

        public override void Move(float deltaX, float deltaY)
        {
            base.Move(deltaX, deltaY);

            if (drawObjects != null)
                for (int i = 0; i < drawObjects.Count; i++)
                {
                    drawObjects[i].Move(deltaX, deltaY);
                }
            if (deviceDrawObjectBases != null)
                for (int i = 0; i < deviceDrawObjectBases.Count; i++)
                {
                    deviceDrawObjectBases[i].Move(deltaX, deltaY);
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
            SymbolUnit._Dic.TryGetValue(_hrefId, out SymbolUnit value);
            if (value != null)
            {
                
                s += "<g id=\"" + _EntityId + "\">";

                s += "<use "+"x=\""+rectangle.X+"\""+" y=\"" + rectangle.Y + "\""+" width=\"" + rectangle.Width + "\""+" height=\"" +
                    rectangle.Height + "\"" + " fill=\"" + Fill + "\""+" xlink:href=\"#" + _hrefId + "\"";

                s += "</g>";
            }
            
            return s;

        }
    }
}
