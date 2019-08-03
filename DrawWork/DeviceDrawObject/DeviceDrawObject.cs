using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawWork.Animation;

namespace DrawWork
{
    //设备抽象类
    public abstract class DeviceDrawObject : DrawRectangleObject
    {
        #region 字段

        protected string _deviceID;//设备Id

        protected int _devicestate;

        protected Dictionary<int, List<DeviceDrawObject>> handledevice;

        /// <summary>
        /// 简单图形的id和动画列表的映射
        /// </summary>
        protected Dictionary<string, List<AnimationBase>> _animationDic;

        #endregion 字段

        #region 构造器

        public DeviceDrawObject()
        {
            handledevice = new Dictionary<int, List<DeviceDrawObject>>();
            _animationDic = new Dictionary<string, List<AnimationBase>>();
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
        protected virtual string GetAllSimpleXML()
        {

            //获取所有基本图形的type 和 id 例如 rect|line  id="1"
            svg
            string id = default;
            //其次通过id(string)查找有无动画绑定
            _animationDic
            return "";
        }



        protected virtual void SetAnimation(string id, AnimationBase animationBase)
        {
            List<AnimationBase> list = null;
            if (!_animationDic.TryGetValue(id, out list))
            {
                list = new List<AnimationBase>();
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
