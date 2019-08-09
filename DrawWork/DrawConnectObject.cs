using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork
{
    /// <summary>
    /// 连接线
    /// </summary>
    public class DrawConnectObject : DrawPathObject
    {
        protected DeviceDrawObjectBase startDrawObject;
        protected DeviceDrawObjectBase endDrawObject;


        public DrawConnectObject(float x, float y) : base(x,y)
        {

        }

        public DrawConnectObject(String[] arr) : base(arr)
        {

        }

        public string StartHrefId
        {
            get
            {
                
                return startDrawObject?._hrefId;
            }
        }

        public string EndHrefId => endDrawObject?._hrefId;

        public void SetFollowDrawObject(int handleNumber, DeviceDrawObjectBase draw)
        {
            if (handleNumber <= 1)
            {
                handleNumber = 1;
                startDrawObject = draw;
            }

            if (handleNumber >= _pointArray.Count)
            {
                handleNumber = _pointArray.Count;
                endDrawObject = draw;
            }

           
        }

        

        public override void Draw(Graphics g)
        {
            

            IEnumerator enumerator = _pointArray.GetEnumerator();

            while (enumerator.MoveNext())
            {
                if (enumerator.Current != null)
                    switch (((PathCommands) enumerator.Current).Pc)
                    {
                        case 'M':
                            if (startDrawObject != null)
                                if (enumerator.Current != null && startDrawObject != null && startDrawObject.drawObjects[0] is DrawCircleObject cirstar)
                                    ((PathCommands) enumerator.Current).P = cirstar.GetWorldDrawObject().GetCenter();
                            break;


                        case 'Z':
                            if (enumerator.Current != null && endDrawObject != null && endDrawObject.drawObjects[0] is DrawCircleObject cirend)
                                ((PathCommands) enumerator.Current).P = cirend.GetWorldDrawObject().GetCenter();
                            break;
                        default:
                            break;
                    }
            }
            base.Draw(g);
        }
        /// <summary>
        /// 连接线序列化成xml
        /// 包含连接的物体信息，复现时需读取
        /// </summary>
        /// <param name="scale"></param>
        /// <param name="noanimation"></param>
        /// <returns></returns>
        public override string GetXmlStr(SizeF scale, bool noanimation = true)
        {
            //编写metadata

            return null;
        }
    }
}
