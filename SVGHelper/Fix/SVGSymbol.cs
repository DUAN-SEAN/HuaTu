using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SVGHelper.Base;

namespace SVGHelper
{
    public class SVGSymbol:SVGUnit
    {

        [Category("(Specific)")]
        [Description("The x-axis coordinate of the side of the element which has the smaller x-axis coordinate value in the current user coordinate system. If the attribute is not specified, the effect is as if a value of 0 were specified.")]
        public string ViewBox
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrSymbol_viewBox);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrSymbol_viewBox,"");
            }
        }




        protected SVGSymbol(SVGWord doc) : base(doc)
        {
            Init();
        }

        private void Init()
        {
            m_sElementName = "symbol";

            m_ElementType = SVGUnitType.device;

            AddAttr(SVGAttribute._SvgAttribute.attrSymbol_viewBox,"");


        }
    }
}
