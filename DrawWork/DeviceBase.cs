using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork
{
    


    public class DeviceUnit
    {
        /// <summary>
        /// 设备引用集合
        /// </summary>
        protected List<DeviceUnit> _useDevices;

        /// <summary>
        /// 设备图素集合
        /// </summary>
        protected List<DrawObject> _drawObjects;

        
        public virtual void Draw(Graphics g)
        {
            foreach (var drawObject in _drawObjects)
            {
                drawObject.Draw(g);
            }

            foreach (var useDevice in _useDevices)
            {
                useDevice.Draw(g);
            }
        }


    }
}
