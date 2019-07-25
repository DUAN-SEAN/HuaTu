using SVGHelper.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGHelper
{
    public class SVGEllipse : SVGBaseShape
    {
        [Category("(Specific)")]
        [Description("The x-axis coordinate of the center of the ellipse.")]
        public string CX
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrSpecific_CX);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrSpecific_CX, value);
            }
        }

        
        [Category("(Specific)")]
        [Description("The y-axis coordinate of the center of the ellipse.")]
        public string CY
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrSpecific_CY);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrSpecific_CY, value);
            }
        }

        [Category("(Specific)")]
        [Description("The x-axis radius of the ellipse.")]
        public string RX
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrSpecific_RX);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrSpecific_RX, value);
            }
        }

       
        [Category("(Specific)")]
        [Description("The y-axis radius of the ellipse.")]
        public string RY
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrSpecific_RY);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrSpecific_RY, value);
            }
        }

       
        public SVGEllipse(SVGWord doc) : base(doc)
        {
            Init();
        }

        
        public SVGEllipse(SVGWord doc, string sCX, string sCY, string sRX, string sRY) : base(doc)
        {
            Init();

            CX = sCX;
            CY = sCY;
            RX = sRX;
            RY = sRY;
        }

        private void Init()
        {
            m_sElementName = "ellipse";
            m_ElementType = SVGUnitType.typeEllipse;

            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_CX, "");
            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_CY, "");
            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_RX, "");
            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_RY, "");
        }
    }
}
