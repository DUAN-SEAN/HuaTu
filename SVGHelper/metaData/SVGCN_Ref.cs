using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SVGHelper.Base;

namespace SVGHelper.metaData
{
     public class SVGCN_Ref:SVGUnit
    {
        [Category("(Specific)")]
        [Description("The x-axis coordinate of the center of the ellipse.")]
        public string LinkObjecttlDnd
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrLinkObjecttlDnd);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrLinkObjecttlDnd, value);
            }
        }
        public string LinkObjectIDznd
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrLinkObjectIDznd);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrLinkObjectIDznd, value);
            }
        }
        public SVGCN_Ref(SVGWord doc) : base(doc)
        {
            Init();
        }

        private void Init()
        {

            m_sElementName = "cge:CN_Ref";
            m_ElementType = SVGUnitType.CN_Ref;

            AddAttr(SVGAttribute._SvgAttribute.attrLinkObjecttlDnd,"");
            AddAttr(SVGAttribute._SvgAttribute.attrLinkObjectIDznd,"");
        }
    }
}
