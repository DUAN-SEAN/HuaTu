using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SVGHelper.Base;

namespace SVGHelper
{
    /// <summary>
    /// 引用元素
    /// </summary>
    public class SVGUse:SVGUnit
    {
        [Category("(Specific)")]
        [Description("The x-axis coordinate of the side of the element which has the smaller x-axis coordinate value in the current user coordinate system. If the attribute is not specified, the effect is as if a value of 0 were specified.")]
        public string X
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrSpecific_X);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrSpecific_X, value);
            }
        }

        [Category("(Specific)")]
        [Description("The y-axis coordinate of the side of the element which has the smaller y-axis coordinate value in the current user coordinate system. If the attribute is not specified, the effect is as if a value of 0 were specified.")]
        public string Y
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrSpecific_Y);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrSpecific_Y, value);
            }
        }

        [Category("(Specific)")]
        [Description("The width of the element. A value of zero disables rendering of the element.")]
        public string Width
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrSpecific_Width);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrSpecific_Width, value);
            }
        }

        [Category("(Specific)")]
        [Description("The height of the element. A value of zero disables rendering of the element.")]
        public string Height
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrSpecific_Height);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrSpecific_Height, value);
            }
        }

        [Category("(Specific)")]
        [Description("The height of the element. A value of zero disables rendering of the element.")]
        public string HRef
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrXLink_HRef);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrXLink_HRef, value);
            }
        }

        protected SVGUse(SVGWord doc) : base(doc)
        {
            Init();

        }

        private void Init()
        {
            m_sElementName = "use";
            m_ElementType = SVGUnitType.use;

            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_X,"");
            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_Y,"");
            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_Width, "");
            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_Height, "");
            AddAttr(SVGAttribute._SvgAttribute.attrXLink_HRef,"");
            AddAttr(SVGAttribute._SvgAttribute.attrSysmbol_terminal_index,"");


        }
    }
}
