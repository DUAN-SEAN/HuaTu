using SVGHelper.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGHelper
{
    public class SVGCircle : SVGBaseShape
    {
        [Category("(Specific)")]
        [Description("The x-axis coordinate of the center of the circle.")]
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
        [Description("The y-axis coordinate of the center of the circle.")]
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
        [Description("The radius of the circle.")]
        public string R
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrSpecific_R);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrSpecific_R, value);
            }
        }

        public SVGCircle(SVGWord doc) : base(doc)
        {
            Init();
        }

        public SVGCircle(SVGWord doc, string sCX, string sCY, string sRadius) : base(doc)
        {
            Init();

            CX = sCX;
            CY = sCY;
            R = sRadius;
        }

        private void Init()
        {
            m_sElementName = "circle";
            m_ElementType = SVGUnitType.typeCircle;

            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_CX, "");
            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_CY, "");
            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_R, "");
        }
    }
}
