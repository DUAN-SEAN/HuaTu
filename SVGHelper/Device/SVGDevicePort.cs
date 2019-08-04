using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SVGHelper.Base;

namespace SVGHelper
{
    public class SVGDevicePort:SVGUnit
    {
        public SVGDevicePort(SVGWord doc) : base(doc)
        {

            Init();
        }

        private void Init()
        {
            m_sElementName = "devicePort";
            m_ElementType = SVGUnitType.devicePort;

            AddAttr(SVGAttribute._SvgAttribute.attrDevicePort_OnwerId,"");
            AddAttr(SVGAttribute._SvgAttribute.attrDevicePort_ConnectId,"");

        }
    }
}
