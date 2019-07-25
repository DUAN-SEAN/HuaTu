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
    public class SVGLine : SVGBaseShape
    {
        [Category("(Specific)")]
        [Description("The x-axis coordinate of the start point of the line.")]
        public string X1
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrSpecific_X1);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrSpecific_X1, value);
            }
        }


        [Category("(Specific)")]
        [Description("The y-axis coordinate of the start point of the line.")]
        public string Y1
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrSpecific_Y1);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrSpecific_Y1, value);
            }
        }

   
        [Category("(Specific)")]
        [Description("The x-axis coordinate of the end point of the line.")]
        public string X2
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrSpecific_X2);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrSpecific_X2, value);
            }
        }

        [Category("(Specific)")]
        [Description("The y-axis coordinate of the end point of the line.")]
        public string Y2
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrSpecific_Y2);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrSpecific_Y2, value);
            }
        }

        public SVGLine(SVGWord doc) : base(doc)
        {
            Init();
        }

       
        public SVGLine(SVGWord doc, string sX1, string sY1, string sX2, string sY2, Color col) : base(doc)
        {
            Init();

            X1 = sX1;
            Y1 = sY1;
            X2 = sX2;
            Y2 = sY2;
            Fill = col;
        }

        private void Init()
        {
            m_sElementName = "line";
            m_ElementType = SVGUnitType.typeLine;

            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_X1, "");
            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_Y1, "");
            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_X2, "");
            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_Y2, "");
        }
    }
}
