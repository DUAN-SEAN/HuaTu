using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork
{
   public class PortDrawObject:DrawCircleObject
   {
       public DeviceDrawObject OwnerDevice;//所属设备

       public DeviceDrawObject ConnectDevice;//连接的设备

       public override string GetXmlStr(SizeF scale, bool noAnimation = true)
       {
            string s = "";
            s += "<devicePort";
            if (OwnerDevice != null)
                s += " onwerId=\"" + OwnerDevice.Id+ "\"";
            if (ConnectDevice != null)
                s += " connectId=\"" + ConnectDevice.Id + "\"";

            s += " >" + "\r\n";
            //添加圆形图案
            
            if(base.AnimationBases==null||base.AnimationBases.Count==0)
                s += base.GetXmlStr(scale, true);
            else
            {
                s += base.GetXmlStr(scale, false);
                s += GetAnimationXML();
            }

            s += "</devicePort>";
            return s;
       }
   }
}
