using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawWork.Command;
using SVGHelper;
using SVGHelper.Base;

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

        public static DrawConnectObject Create(SVGPath svg)
        {
            DrawConnectObject dp;

            try
            {
                string s = svg.PathData.Trim();
                s = s.Replace("\r", "");
                s = s.Replace("\n", " ");
                s = s.Trim();
                string[] arr = s.Split(' ');

                dp = new DrawConnectObject(arr) { Name = svg.ShapeName };
                dp.SetStyleFromSvg(svg);
            }
            catch (Exception ex)
            {
                SVGErr.Log("DrawPathObject", "Create", ex.ToString(), SVGErr._LogPriority.Info);
                dp = null;
            }

            return dp;
        }


        /// <summary>
        /// 通过端口ID连接端口
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="index"></param>
        /// <param name="deviceDrawObjectBases"></param>
        public void SetConnectDeviceFromXml(string deviceId, int index,List<DeviceDrawObjectBase> deviceDrawObjectBases)
        {
            for (int i = 0; i < deviceDrawObjectBases.Count; i++)
            {
                if (deviceDrawObjectBases[i] is DeviceDrawObjectBase device)
                {
                    if (device.GetDeviceDrawObjectBydeviceId(deviceId,out var port))
                    {
                        SetFollowDrawObject(index, port);
                        break;
                    }
                }
            }
        }
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
        /// 连接线的svg逻辑单独写
        /// </summary>
        /// <param name="scale"></param>
        /// <param name="noanimation"></param>
        /// <returns></returns>
        public override string GetXmlStr(SizeF scale, bool noanimation = true)
        {
            //编写metadata
            string start = startDrawObject != null ? startDrawObject._EntityId : "";
            string end = endDrawObject != null ? endDrawObject._EntityId : "";
            string s = base.GetXmlStr(scale, noanimation);
            s += "<metadata>" + "\r\n";
            s += "<cge:CN_Ref LinkObjectlDnd=\"" + start + "\"" + " LinkObjectIDznd=\"" + end + "\"/>";
            s += "\r\n</metadata>";
            return s;
        }
    }
}
