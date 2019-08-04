using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SVGHelper.Base;

namespace SVGHelper.Device
{
    public class SVGDevice:SVGUnit
    {
        [Category("(Specific)")]
        [Description("The x-axis coordinate of the side of the element which has the smaller x-axis coordinate value in the current user coordinate system. If the attribute is not specified, the effect is as if a value of 0 were specified.")]
        public string DeviceType
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrDevice_Type);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrDevice_Type, value);
            }
        }


        protected SVGDevice(SVGWord doc) : base(doc)
        {
            Init();
        }

        private void Init()
        {

            m_sElementName = "device";
            m_ElementType = SVGUnitType.device;

            AddAttr(SVGAttribute._SvgAttribute.attrDevice_Type,"");
        }
    }
}
