using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGHelper.Base
{
    public class SVGGroup : SVGUnit
    {
        /// <summary>
        /// 它构造一个没有属性的组元素。
        /// </summary>
        /// <param name="doc">SVG document.</param>
        public SVGGroup(SVGWord doc) : base(doc)
        {
            m_sElementName = "g";
            m_ElementType = SVGUnitType.typeGroup;
        }
    }
}
