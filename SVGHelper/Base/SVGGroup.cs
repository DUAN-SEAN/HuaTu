using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGHelper.Base
{
    public class SVGGroup : SVGUnit
    {
        [Category("(Specific)")]
        [Description("For Group Transform")]
        public string Transform
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrSpecific_Transform);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrSpecific_Transform, value);
            }
        }

        /// <summary>
        /// 它构造一个没有属性的组元素。
        /// </summary>
        /// <param name="doc">SVG document.</param>
        public SVGGroup(SVGWord doc) : base(doc)
        {
            Init();
           
            
        }

        private void Init()
        {
            m_sElementName = "g";
            m_ElementType = SVGUnitType.typeGroup;

            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_Transform, "");
        }
    }
}
