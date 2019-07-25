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
    public class SVGRect : SVGBaseShape
    {
		[Category("(Specific)")]
        [Description("X-axis coordinate of the side of the element which has the smaller x-axis coordinate value in the current user coordinate system. If the attribute is not specified, the effect is as if a value of 0 were specified.")]
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
        [Description("Y-axis coordinate of the side of the element which has the smaller y-axis coordinate value in the current user coordinate system. If the attribute is not specified, the effect is as if a value of 0 were specified.")]
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
        [Description("Width of the element. A value of zero disables rendering of the element.")]
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
        [Description("Height of the element. A value of zero disables rendering of the element.")]
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
        [Description("For rounded rectangles, the x-axis radius of the ellipse used to round off the corners of the rectangle.")]
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
        [Description("For rounded rectangles, the y-axis radius of the ellipse used to round off the corners of the rectangle.")]
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

        public SVGRect(SVGWord doc) : base(doc)
        {
            Init();
        }

        public SVGRect(SVGWord doc,
                       string sX,
                       string sY,
                       string sWidth,
                       string sHeight,
                       string sStrokeWidth,
                       Color colFill,
                       Color colStroke) : base(doc)
        {
            Init();

            X = sX;
            Y = sY;
            Width = sWidth;
            StrokeWidth = sStrokeWidth;
            Height = sHeight;
            Fill = colFill;
            Stroke = colStroke;
        }

        private void Init()
        {
            m_sElementName = "rect";
            m_ElementType = SVGUnitType.typeRect;

            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_ShapeName, null);
            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_X, null);
            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_Y, null);
            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_Width, null);
            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_Height, null);
            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_RX, null);
            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_RY, null);
        }
    }
}
