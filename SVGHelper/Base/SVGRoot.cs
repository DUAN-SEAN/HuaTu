using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGHelper.Base
{
    public class SVGRoot : SVGUnit
    {
        /// <summary>
        /// Standard XML namespace.
        /// </summary>
        [Category("Svg")]
        [Description("Standard XML namespace.")]
        public string XmlNs
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrSvg_XmlNs);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrSvg_XmlNs, value);
            }
        }

        /// <summary>
        /// Standard XML version.
        /// </summary>
        [Category("Svg")]
        [Description("Standard XML version.")]
        public string Version
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrSvg_Version);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrSvg_Version, value);
            }
        }

        /// <summary>
        /// The width of the svg area.
        /// </summary>
        [Category("(Specific)")]
        [Description("The width of the svg area.")]
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

        /// <summary>
        /// The height of the svg area.
        /// </summary>
        [Category("(Specific)")]
        [Description("The height of the svg area.")]
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

        internal SVGRoot(SVGWord doc) : base(doc)
        {
            m_sElementName = "svg";
            m_ElementType = SVGUnitType.typeSvg;

            AddAttr(SVGAttribute._SvgAttribute.attrSvg_XmlNs, "");
            AddAttr(SVGAttribute._SvgAttribute.attrSvg_Version, "");

            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_Width, "");
            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_Height, "");
        }
    }
}
