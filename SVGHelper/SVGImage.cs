using SVGHelper.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGHelper
{
    public class SVGImage : SVGUnit
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

        [Category("Style")]
        [Description("This attribute assigns a (CSS) class name or set of class names to an element.")]
        public string Class
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrStyle_Class);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrStyle_Class, value);
            }
        }

        [Category("Style")]
        [Description("This attribute specifies style information for the current element. The style attribute specifies style information for a single element.")]
        public string Style
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrStyle_Style);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrStyle_Style, value);
            }
        }

        [Category("(Specific)")]
        [Description("Url of the image file.")]
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

       
        public SVGImage(SVGWord doc) : base(doc)
        {
            Init();
        }

       
        public SVGImage(SVGWord doc,
                        string sX,
                        string sY,
                        string sWidth,
                        string sHeight,
                        string sHRef) : base(doc)
        {
            Init();

            X = sX;
            Y = sY;
            Width = sWidth;
            Height = sHeight;
            HRef = sHRef;
        }

        private void Init()
        {
            m_sElementName = "image";
            m_ElementType = SVGUnitType.typeImage;

            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_X, "");
            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_Y, "");
            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_Width, "");
            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_Height, "");

            AddAttr(SVGAttribute._SvgAttribute.attrXLink_HRef, "");

            AddAttr(SVGAttribute._SvgAttribute.attrStyle_Class, "");
            AddAttr(SVGAttribute._SvgAttribute.attrStyle_Style, "");
        }
    }
}
