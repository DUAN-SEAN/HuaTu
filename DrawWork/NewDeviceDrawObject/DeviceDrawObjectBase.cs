using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        public string _hrefId; //引用设备id

        public float ViewBox_w;

        public float ViewBox_h;

        public DeviceDrawObjectBase()
        {
            SetRectangleF(0, 0, 1, 1);

            Initialize();
        }

        public void SetViewBox(float w, float h)
        {
            ViewBox_w = w;
            ViewBox_h = h;
        }

        public DeviceDrawObjectBase(float x, float y, float width, float height, string entityId,
            List<DrawObject> drawobjs, List<DeviceDrawObjectBase> deviceDrawObjectBases, string hrefId)
        {
            //这里的矩形是画布的位置和长宽
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;
            _EntityId = entityId;
            drawObjects = drawobjs;
            this.deviceDrawObjectBases = deviceDrawObjectBases;

            float left = rectangle.Left;
            float top = rectangle.Top;
            float right = rectangle.Right;
            float bottom = rectangle.Bottom;

            //if (deviceDrawObjectBases != null)
            //    foreach (var deviceDrawObjectBase in deviceDrawObjectBases)
            //    {
            //        for (int i = 0; i < deviceDrawObjectBase.HandleCount; i++)
            //        {
            //            var point = deviceDrawObjectBase.GetHandle(i);
            //            if (point.X < left) left = point.X;
            //            if (point.X > right) right = point.X;
            //            if (point.Y < top) top = point.Y;
            //            if (point.Y > bottom) bottom = point.Y;
            //        }
            //    }

            //if (drawobjs != null)
            //    foreach (var drawObject in drawobjs)
            //    {
            //        for (int i = 0; i < drawObject.HandleCount; i++)
            //        {
            //            var point = drawObject.GetHandle(i);
            //            if (point.X < left) left = point.X;
            //            if (point.X > right) right = point.X;
            //            if (point.Y < top) top = point.Y;
            //            if (point.Y > bottom) bottom = point.Y;
            //        }
            //    }

            if (deviceDrawObjectBases != null)
                foreach (var deviceDrawObjectBase in deviceDrawObjectBases)
                {
                    //deviceDrawObjectBase.ParentPointF = new PointF(left, top);
                    deviceDrawObjectBase.SetParent(this);
                  //  deviceDrawObjectBase.Proportion = new PointF(deviceDrawObjectBase.Width / right - left,
                    //    deviceDrawObjectBase.Height / bottom - top);

                }

            if (drawobjs != null)
                foreach (var drawObject in drawobjs)
                {
                    //drawObject.ParentPointF = new PointF(left, top);
                    drawObject.SetParent(this);

                    //if (drawObject is DrawRectangleObject rectangleObject)
                       // rectangleObject.Proportion = new PointF(rectangleObject.Width / right - left,
                       //     rectangleObject.Height / bottom - top);

                }

           // SetRectangleF(left, top, right - left, bottom - top);


            this._hrefId = hrefId;





            Initialize();
        }
    

    #region DrawMethod
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
            base.Rotate(angle);
        }

        public override void Draw(Graphics g)
        {
            if (drawObjects != null)
                for (int i = 0; i < drawObjects.Count; i++)
                {
                    drawObjects[i].ParentPointF = new PointF(ParentAndRectangleF.X, ParentAndRectangleF.Y);
                    drawObjects[i].Draw(g);
                }
            if (deviceDrawObjectBases != null)
                for (int i = 0; i < deviceDrawObjectBases.Count; i++)
                {
                    deviceDrawObjectBases[i].ParentPointF = new PointF(ParentAndRectangleF.X, ParentAndRectangleF.Y);
                    deviceDrawObjectBases[i].Draw(g);
                }
        }

        public override void Resize(SizeF newscale, SizeF oldscale)
        {

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
            base.Resize(newscale, oldscale);
        }

        //public override void MoveHandl eTo(PointF point, int handleNumber)
        //{
        //    if (drawObjects != null)
        //        for (int i = 0; i < drawObjects.Count; i++)
        //        {

                    //if (drawObjects[i] is DrawRectangleObject drectangle)
                    //{
                    //    var center = GetHandle(handleNumber);
                    //    var drawcenter =  drawObjects[i].GetHandle(handleNumber);
                    //    var xdis = point.X - center.X;
                    //    var ydis = point.Y - center.Y;
                    //    var pro = drawObjects[i].Proportion;

        //                drawObjects[i].MoveHandleTo(
        //                    new PointF(drawcenter.X + xdis * pro.X, drawcenter.Y + ydis * pro.Y),
        //                    handleNumber);

        //                if (drectangle.Rectangle.X + xdis * pro.X > rectangle.X &&
        //                    drectangle.Rectangle.X + xdis * pro.X + drectangle.Rectangle.Width < rectangle.X + Width &&
        //                    drectangle.Rectangle.Y + ydis * pro.Y > rectangle.Y &&
        //                    drectangle.Rectangle.Y + ydis * pro.Y + drectangle.Rectangle.Height < rectangle.Y + Height)
        //                {
        //                    //中心位置拉伸

        //                    drectangle.SetRectangleF(drectangle.Rectangle.X + xdis * pro.X,
        //                        drectangle.Rectangle.Y + ydis * pro.Y, drectangle.Width, drectangle.Height);
                        
                            
                    
        //        }
        //    if (deviceDrawObjectBases != null)
        //        for (int i = 0; i < deviceDrawObjectBases.Count; i++)
        //        {
        //            if (deviceDrawObjectBases[i] != null)
        //            {

        ////                var center = GetHandle(handleNumber);
        ////                var drawcenter = deviceDrawObjectBases[i].GetHandle(handleNumber);
        ////                var xdis = point.X - center.X;
        ////                var ydis = point.Y - center.Y;
        ////                var pro = deviceDrawObjectBases[i].Proportion;

        ////                deviceDrawObjectBases[i].MoveHandleTo(
        ////                    new PointF(drawcenter.X + xdis * pro.X, drawcenter.Y + ydis * pro.Y),
        ////                    handleNumber);

        ////                if (deviceDrawObjectBases[i].Rectangle.X + xdis * pro.X > rectangle.X &&
        ////                    deviceDrawObjectBases[i].Rectangle.X + xdis * pro.X +
        ////                    deviceDrawObjectBases[i].Rectangle.Width < rectangle.X + Width &&
        ////                    deviceDrawObjectBases[i].Rectangle.Y + ydis * pro.Y > rectangle.Y &&
        ////                    deviceDrawObjectBases[i].Rectangle.Y + ydis * pro.Y +
        ////                    deviceDrawObjectBases[i].Rectangle.Height < rectangle.Y + Height)
        ////                {
        ////                    //中心位置拉伸

        ////                    deviceDrawObjectBases[i].SetRectangleF(deviceDrawObjectBases[i].Rectangle.X + xdis * pro.X,
        ////                        deviceDrawObjectBases[i].Rectangle.Y + ydis * pro.Y, rectangle.Width, rectangle.Height);
        ////                }

        //            }
        //        }
        //    base.MoveHandleTo(point, handleNumber);
        //}



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

                s += "<use " + "x=\"" + rectangle.X + "\"" + " y=\"" + rectangle.Y + "\"" + " width=\"" +
                     rectangle.Width + "\"" + " height=\"" +
                     rectangle.Height + "\"" + " fill=\"" + Fill + "\"" + " xlink:href=\"#" + _hrefId + "\"";
                s += GetTransformXML(_angle,GetCenter());
                s+="/>";

                s += "</g>";
                s += "\r\n";
            }
            
            return s;

        }

#endregion




        #region Helper



        #endregion

        public DeviceDrawObjectBase GetNearestPort(PointF getHandle)
        {
            float dis = float.MaxValue;
            int min = -1;
            DeviceDrawObjectBase obj = null;
            if(deviceDrawObjectBases != null)
            {
                for (int i = 0; i < deviceDrawObjectBases.Count; i++)
                {
                    if(deviceDrawObjectBases[i]._hrefId != "Port:端子") continue;

                    var point = deviceDrawObjectBases[i].GetWorldDrawObject().GetCenter();
                    var disnow = (point.X - getHandle.X) * (point.X - getHandle.X) +
                                 (point.Y - getHandle.Y) * (point.Y - getHandle.Y);
                    if (disnow < dis)
                    {
                        dis = disnow;
                        min = i;

                    }
                }

                if (min != -1)
                    obj = deviceDrawObjectBases[min];
            }

            return obj;
            

        }
    }
}
