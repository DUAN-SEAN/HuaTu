using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork
{
    public class PortDrawObject : DrawCircleObject
    {
        public string deviceType = DeviceDrawType.DevicePort; //设备类型

        private DeviceDrawObject _ownerDeviceDrawObject;

        private int _ownerDeviceHandle;

        private DeviceDrawObject _connectDeviceDrawObject;


        public PortDrawObject(float x, float y) : base()
        {
            rectangle.X = x;
            rectangle.Y = y;

            rectangle.Width = 10;
            rectangle.Height = 10;
            Initialize();
        }

        public PortDrawObject(float x, float y, float width, float height) : base()
        {
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;
            Initialize();
        }

        public override int HandleCount
        {
            get { return 9; }
        }

        #region 设备设置

        public DeviceDrawObject OwnerDevice
        {
            set => _ownerDeviceDrawObject = value;
            get => _ownerDeviceDrawObject;
        } //所属设备

        public int OwnerDeviceHandle
        {
            set => _ownerDeviceHandle = value;
            get => _ownerDeviceHandle;
        }

        public DeviceDrawObject ConnectDevice
        {
            set => _connectDeviceDrawObject = value;
            get => _connectDeviceDrawObject;
        } //连接的设备

        #endregion

        public override PointF GetHandle(int handleNumber)
        {
            switch (handleNumber)
            {
                case 9:
                    return GetCenter();

            }

            return base.GetHandle(handleNumber);
        }

        private string GetDeviceType()
        {
            return " deviceType=\"" + deviceType + "\" ";
        }

        public override string GetXmlStr(SizeF scale, bool noAnimation = true)
        {
            string s = "";
            s += "<g ";
            if (OwnerDevice != null)
                s += " onwerId=\"" + OwnerDevice.DeviceId + "\"";
            if (ConnectDevice != null)
                s += " connectId=\"" + ConnectDevice.DeviceId + "\"";

            s += GetDeviceType();
            s += " >" + "\r\n";
            //添加圆形图案

            if (base.AnimationBases == null || base.AnimationBases.Count == 0)
                s += base.GetXmlStr(scale, true);
            else
            {
                s += base.GetXmlStr(scale, false);
                s += GetAnimationXML();
            }

            s += "</g>";
            return s;
        }

        protected override PointF GetCenter()
        {
            var point = _ownerDeviceDrawObject.GetHandle(_ownerDeviceHandle);
            return new PointF(point.X, point.Y);
        }

        public override void Update()
        {
            rectangle.X = _ownerDeviceDrawObject.GetHandle(_ownerDeviceHandle).X - rectangle.Width / 2;
            rectangle.Y = _ownerDeviceDrawObject.GetHandle(_ownerDeviceHandle).Y - rectangle.Height / 2;
        }
    }

}
