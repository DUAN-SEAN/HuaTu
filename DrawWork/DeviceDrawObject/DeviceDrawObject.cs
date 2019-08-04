using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawWork.Animation;

namespace DrawWork
{
    public static class DeviceDrawType
    {
        public const string SingleDisConnectorDrawObject = "刀闸01";

        public const string WireConnectLineDrawObject = "电线01";

    }


    //设备抽象类
    public abstract class DeviceDrawObject : DrawRectangleObject
    {
        private const string Tag = "device";
        #region 字段

        protected List<DrawObject> drawObjects;//组成设备的最小图形，每个设备图元都是固定的

        public string deviceType;//设备类型


        protected string _deviceID;//设备Id

        protected int _devicestate;

        protected Dictionary<int, List<DeviceDrawObject>> handledevice;


        #endregion 字段

        #region 构造器

        public DeviceDrawObject()
        {
            handledevice = new Dictionary<int, List<DeviceDrawObject>>();
            drawObjects = new List<DrawObject>();
        }
        

        #endregion
        #region 虚函数
        /// <summary>
        /// 根据句柄设置被连接的点和对象
        /// </summary>
        /// <param name="handlepoint"></param>
        /// <param name="o"></param>
        public virtual void SetConnect(int handlepoint, DeviceDrawObject o)
        {
            if (handledevice.TryGetValue(handlepoint, out List<DeviceDrawObject> devices))
            {
                devices.Add(o);
            }
            else
            {
                devices = new List<DeviceDrawObject> {o};
                handledevice.Add(handlepoint,devices);
            }

        }

        public override void Update()
        {
            foreach (var VARIABLE in handledevice.Values)
            {
                foreach (var VARIABLE2 in VARIABLE)
                {
                    VARIABLE2.DeviceState = DeviceState;
                }
            }
            //由于不在list中 所以由图元驱动图形的update
            foreach (var doj in drawObjects)
            {
                doj.Update();
            }
        }

        //获取设备id
        public virtual string GetDeviceId()
        {
            return "id=\""+_deviceID+"+\"";
        }
        /// <summary>
        /// 获取图元下所有简单图形的动画状态
        /// </summary>
        /// <returns></returns>
        protected virtual string GetAllSimpleXML(SizeF scale)
        {

            //获取所有基本图形的type 和 id 例如 rect|line  id="1"
            string s = "";
            foreach (var doj in drawObjects)
            {
                //其次通过id(string)查找有无动画绑定
                List<Animation.Animation> list = doj.AnimationBases;
                if (list == null || list.Count == 0)
                {
                    s += doj.GetXmlStr(scale,true);
                }
                else
                {
                    s += doj.GetXmlStr(scale, false);
                    s += doj.GetAnimationXML();
                }


            }
           
            return s;
        }
        /// <summary>
        /// 序列化，同时将图元下的图形和动画都序列化成svg
        /// </summary>
        /// <param name="scale"></param>
        /// <param name="noAnimation"></param>
        /// <returns></returns>
        public override string GetXmlStr(SizeF scale,bool noAnimation = true)
        {
            //用group包裹
            //  <rect x="1" y="1" width="1198" height="398"
            //		style="fill:none; stroke:blue"/>

            string s = "<device ";
            s += GetDeviceId();//获取设备id
            s += GetTransformXML(_angle, fixedCenter);//获取旋转
            s += GetDeviceType();
            s += " >" + "\r\n";

            s += GetAllSimpleXML(scale);//获取所有基础图形的参数以及动画




            s += "</device>";



            return s;
        }

        private string GetDeviceType()
        {
            return " deviceType=\"" + deviceType + "\" ";
        }

        /// <summary>
        /// 给doj设置动画
        /// </summary>
        /// <param name="id"></param>
        /// <param name="animationBase"></param>
        protected void SetAnimation(string id, Animation.Animation animationBase)
        {
           


            foreach (var doj in drawObjects)
            {
                if (id == doj.Id.ToString())
                {
                    if(doj.AnimationBases==null)
                        doj.AnimationBases = new List<Animation.Animation>();

                    doj.AnimationBases.Add(animationBase);
                }
            }

        }
        #endregion 虚函数

        #region 属性
        /// <summary>
        /// 0 未通电
        /// 1 通电
        /// </summary>
        public int DeviceState
        {
            set { _devicestate = value; }
            get { return _devicestate; }
        }

        #endregion 属性
    }

}
