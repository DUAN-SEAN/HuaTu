using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SVGHelper.Base;

namespace SVGHelper
{
    public class SVGDevicePort:SVGUnit
    {
        [Category("(Specific)")]
        [Description("The x-axis coordinate of the side of the element which has the smaller x-axis coordinate value in the current user coordinate system. If the attribute is not specified, the effect is as if a value of 0 were specified.")]
        public string OnwerId
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrDevicePort_OnwerId);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrDevicePort_OnwerId, value);
            }
        }
        [Category("(Specific)")]
        [Description("The x-axis coordinate of the side of the element which has the smaller x-axis coordinate value in the current user coordinate system. If the attribute is not specified, the effect is as if a value of 0 were specified.")]
        public string ConnectId
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrDevicePort_ConnectId);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrDevicePort_ConnectId, value);
            }
        }

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
