using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork
{
    public class DeviceDrawObject : DrawRectangleObject
    {
        #region 字段
        protected int _devicestate;

        protected Dictionary<int, List<DeviceDrawObject>> handledevice;

        #endregion 字段

        #region 构造器

        public DeviceDrawObject()
        {
            handledevice = new Dictionary<int, List<DeviceDrawObject>>();
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
