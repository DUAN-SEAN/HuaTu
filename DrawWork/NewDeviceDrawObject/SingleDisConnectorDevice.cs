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

        protected PointF leftPointF;
        private DrawLineObject centerline;
        public SingleDisConnectorDevice(float x, float y, float width, float height, string entityId,
            List<DrawObject> drawobjs, List<DeviceDrawObjectBase> deviceDrawObjectBases, string hrefId) : base(x, y,
            width, height, entityId, drawobjs, deviceDrawObjectBases, hrefId)
        {

            var cpoint = GetCenter();
            foreach (var drawObject in drawObjects)
            {
                if (drawObject is DrawLineObject line)
                    if (line.GetWorldDrawObject().HitTest(cpoint) >= 0)
                    {
                        centerline = line;
                        leftPointF = line.GetHandle(1);
                    }
            }
        }


        public override int HandleCount => base.HandleCount + 1;

        public override void Draw(Graphics g)
        {
            if (centerline == null)
            {
                var cpoint = GetCenter();
                foreach (var drawObject in drawObjects)
                {
                    if (drawObject is DrawLineObject line)
                        if (line.GetWorldDrawObject().HitTest(cpoint) >= 0)
                        {
                            centerline = line;
                            leftPointF = line.GetHandle(0);
                        }
                }
            }

            foreach (var drawObject in drawObjects)
            {
                if (drawObject == centerline)
                    centerline.MoveHandleTo(isClose ? new PointF(0, leftPointF.Y) : leftPointF,0);
            }
            base.Draw(g);
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
            isOn = !isClose;
        }
    }
}
