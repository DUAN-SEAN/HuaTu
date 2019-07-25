using SVGHelper.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGHelper
{
    public class SVGText : SVGUnit
    {
        
		[Category("(Core)")]
        [Description("Specifies a base URI other than the base URI of the document or external entity.")]
        public string XmlBase
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrCore_XmlBase);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrCore_XmlBase, value);
            }
        }

        [Category("(Core)")]
        [Description("Standard XML attribute to specify the language (e.g., English) used in the contents and attribute values of particular elements.")]
        public string XmlLang
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrCore_XmlLang);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrCore_XmlLang, value);
            }
        }

        [Category("(Core)")]
        [Description("Standard XML attribute to specify whether white space is preserved in character data. The only possible values are default and preserve.")]
        public string XmlSpace
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrCore_XmlSpace);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrCore_XmlSpace, value);
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
        [Category("(Paint)")]
        public Color Fill
        {
            get
            {
                return GetAttributeColorValue(SVGAttribute._SvgAttribute.attrPaint_Fill);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrPaint_Fill, value);
            }
        }

        [Category("(Specific)")]
        [Description("The value of the element.")]
        public string Value
        {
            get
            {
                return m_sElementValue;
            }

            set
            {
                m_sElementValue = value;
            }
        }

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
        [Description("Shifts in the current text position along the x-axis for the characters within this element or any of its descendants.")]
        public string DX
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrSpecific_DX);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrSpecific_DX, value);
            }
        }

        [Category("(Specific)")]
        [Description("Shifts in the current text position along the y-axis for the characters within this element or any of its descendants.")]
        public string DY
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrSpecific_DY);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrSpecific_DY, value);
            }
        }

        [Category("(Specific)")]
        [Description("The supplemental rotation about the current text position that will be applied to all of the glyphs corresponding to each character within this element.")]
        public string Rotate
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrSpecific_Rotate);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrSpecific_Rotate, value);
            }
        }

        [Category("(Specific)")]
        [Description("The author's computation of the total sum of all of the advance values that correspond to character data within this element, including the advance value on the glyph (horizontal or vertical), the effect of properties 'kerning', 'letter-spacing' and 'word-spacing' and adjustments due to attributes dx and dy on 'tspan' elements. This value is used to calibrate the user agent's own calculations with that of the author.")]
        public string TextLength
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrSpecific_TextLength);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrSpecific_TextLength, value);
            }
        }

        [Category("(Specific)")]
        [Description("Indicates the type of adjustments which the user agent shall make to make the rendered length of the text match the value specified on the textLength attribute.")]
        public SVGAttribute._SvgLengthAdjust LengthAdjust
        {
            get
            {
                return (SVGAttribute._SvgLengthAdjust)GetAttributeIntValue(SVGAttribute._SvgAttribute.attrSpecific_LengthAdjust);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrSpecific_LengthAdjust, (int)value);
            }
        }

        [Category("Font")]
        [Description("Indicates which font family is to be used to render the text, specified as a prioritized list of font family names and/or generic family names.")]
        public string FontFamily
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrFont_Family);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrFont_Family, value);
            }
        }

        [Category("Font")]
        [Description("This property refers to the size of the font from baseline to baseline when multiple lines of text are set solid in a multiline layout environment.")]
        public string FontSize
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrFont_Size);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrFont_Size, value);
            }
        }

        [Category("Font")]
        [Description("This property allows authors to specify an aspect value for an element that will preserve the x-height of the first choice font in a substitute font.")]
        public string SizeAdjust
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrFont_SizeAdjust);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrFont_SizeAdjust, value);
            }
        }

        [Category("Font")]
        //		[Description("This property indicates the desired amount of condensing or expansion in the glyphs used to render the text.")]
        public string FontWeight
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrFont_Weight);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrFont_Weight, value);
            }
        }
        [Category("Font")]
        //		[Description("This property indicates the desired amount of condensing or expansion in the glyphs used to render the text.")]
        public string FontStyle
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrFont_Style);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrFont_Style, value);
            }
        }

        [Category("Font")]
        [Description("This property indicates the desired amount of condensing or expansion in the glyphs used to render the text.")]
        public SVGAttribute._SvgFontStretch Stretch
        {
            get
            {
                return (SVGAttribute._SvgFontStretch)GetAttributeIntValue(SVGAttribute._SvgAttribute.attrFont_Stretch);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrFont_Stretch, (int)value);
            }
        }
        [Category("(Specific)")]
        [Description("The y-axis coordinate of the side of the element which has the smaller y-axis coordinate value in the current user coordinate system. If the attribute is not specified, the effect is as if a value of 0 were specified.")]
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
        [Description("The y-axis coordinate of the side of the element which has the smaller y-axis coordinate value in the current user coordinate system. If the attribute is not specified, the effect is as if a value of 0 were specified.")]
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

        public SVGText(SVGWord doc) : base(doc)
        {
            m_sElementName = "text";
            m_bHasValue = true;
            m_ElementType = SVGUnitType.typeText;

            AddAttr(SVGAttribute._SvgAttribute.attrCore_XmlBase, "");
            AddAttr(SVGAttribute._SvgAttribute.attrCore_XmlLang, "");
            AddAttr(SVGAttribute._SvgAttribute.attrCore_XmlSpace, "");
            AddAttr(SVGAttribute._SvgAttribute.attrPaint_Fill, "");

            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_X, "");
            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_Y, "");
            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_DX, "");
            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_DY, "");
            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_Rotate, "");
            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_TextLength, "");
            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_LengthAdjust, 0);
            AddAttr(SVGAttribute._SvgAttribute.attrText_Anchor, "");

            AddAttr(SVGAttribute._SvgAttribute.attrStyle_Class, "");
            AddAttr(SVGAttribute._SvgAttribute.attrStyle_Style, "");

            AddAttr(SVGAttribute._SvgAttribute.attrFont_Family, "");
            AddAttr(SVGAttribute._SvgAttribute.attrFont_Size, "");
            AddAttr(SVGAttribute._SvgAttribute.attrFont_SizeAdjust, "");
            AddAttr(SVGAttribute._SvgAttribute.attrFont_Stretch, 0);
            AddAttr(SVGAttribute._SvgAttribute.attrFont_Style, "");
            AddAttr(SVGAttribute._SvgAttribute.attrFont_Variant, "");
            AddAttr(SVGAttribute._SvgAttribute.attrFont_Weight, "");
        }
        public override void ParseStyle(string sval)
        {
            string[] arr = sval.Split(';');
            for (int i = 0; i < arr.Length; i++)
            {
                string s = arr[i].Trim();
                string[] arrp = s.Split(':');
                if (arrp.Length < 2)
                    continue;
                switch (arrp[0])
                {
                    case "fill":
                    case "font-family":
                    case "font-size":
                    case "font-style":
                    case "font-weight":
                        SetAttributeValue(arrp[0], arrp[1]);
                        break;
                }
            }
        }
        [Category("Font")]
        //		[Description("This property indicates the desired amount of condensing or expansion in the glyphs used to render the text.")]
        public string TextAnchor
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrText_Anchor);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrText_Anchor, value);
            }
        }
    }
}
