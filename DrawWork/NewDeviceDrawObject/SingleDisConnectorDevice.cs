using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.NewDeviceDrawObject
{
    public class SingleDisConnectorDevice : DeviceDrawObjectBase
    {
        /// <summary>
        /// 判断开关是否关闭
        /// </summary>
        protected bool isClose;
        public SingleDisConnectorDevice(float x, float y, float width, float height, string entityId,
            List<DrawObject> drawobjs, List<DeviceDrawObjectBase> deviceDrawObjectBases, string hrefId) : base(x, y,
            width, height, entityId, drawobjs, deviceDrawObjectBases, hrefId)
        {

        }


        public override int HandleCount => base.HandleCount + 1;

        public override void Draw(Graphics g)
        {
            foreach (var drawObject in drawObjects)
            {
                
            }
        }


        public override PointF GetHandle(int handleNumber)
        {
            if (handleNumber <= base.HandleCount)
                return base.GetHandle(handleNumber);
            return GetCenter();
        }


        public override void MouseClickOnHandleUp(int handle)
        {
            if (handle == HandleCount)
                isClose = !isClose;
            ;
        }
    }
}
