using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawWork.Animation;
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
        protected int startportindex;
        protected DeviceDrawObjectBase endDrawObject;
        protected int endportindex;

        /// <summary>
        /// 正在播放的动画
        /// </summary>
        protected List<DrawObject> animationList;
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

        public override void Update()
        {
           if(startDrawObject == null || endDrawObject == null)
           {
               RemoveAnimation();
               return;
           }

           if(startDrawObject.IsOn && endDrawObject.IsOn) 
               AddAnimation();

               
        }
        /// <summary>
        /// 判断到左右点设备都是On
        /// 就添加动画
        /// 已经添加过则不会再重复添加
        /// </summary>
        protected void AddAnimation()
        {
            //TODO 添加动画对象到animationList中
            AnimationPath path = new AnimationPath();



            foreach (var drawObject in animationList)
            {
                DrawObjectList.AddAnimation(drawObject);
            }
        }

        /// <summary>
        /// 判断到有设备是 oFF
        /// 就删除动画
        /// 没有动画则不会重复删除
        /// </summary>
        protected void RemoveAnimation()
        {

            foreach (var drawObject in animationList)
            {
                DrawObjectList.RemoveAnimation(drawObject);
            }
            animationList.Clear();
        }
        /// <summary>
        /// 通过端口ID连接端口
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="index"></param>
        /// <param name="deviceDrawObjectBases"></param>
        public void SetConnectDeviceFromXml(string deviceId, int index,int portIndex,List<DeviceDrawObjectBase> deviceDrawObjectBases)
        {
            for (int i = 0; i < deviceDrawObjectBases.Count; i++)
            {
                if (deviceDrawObjectBases[i] is DeviceDrawObjectBase device)
                {
                    if (device.GetDeviceDrawObjectBydeviceId(deviceId,out var port))
                    {
                        SetFollowDrawObject(index, port,portIndex);
                        break;
                    }
                }
            }
        }
        public void SetFollowDrawObject(int handleNumber, DeviceDrawObjectBase draw,int portindex)
        {
            if (handleNumber <= 1)
            {
                handleNumber = 1;
                startDrawObject = draw;
                startportindex = portindex;
            }

            if (handleNumber >= _pointArray.Count)
            {
                handleNumber = _pointArray.Count;
                endDrawObject = draw;
                endportindex = portindex;
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
                            if (startDrawObject != null && startDrawObject.deviceDrawObjectBases != null)
                                if (enumerator.Current != null && startDrawObject != null && startDrawObject.deviceDrawObjectBases[startportindex].drawObjects[0] is DrawCircleObject cirstar)
                                    ((PathCommands) enumerator.Current).P = cirstar.GetWorldDrawObject().GetCenter();
                            break;


                        case 'Z':
                            if (endDrawObject != null && endDrawObject.deviceDrawObjectBases != null)
                                if (enumerator.Current != null && endDrawObject != null &&
                                    endDrawObject.deviceDrawObjectBases[endportindex].drawObjects[0] is DrawCircleObject
                                        cirend)
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
            s += "<cge:CN_Ref LinkObjectlDnd=\"" + start+"@" +startportindex+ "\"" + " LinkObjectIDznd=\"" + end +"@"+endportindex+ "\"/>";
            s += "\r\n</metadata>";
            return s;
        }
    }
}
