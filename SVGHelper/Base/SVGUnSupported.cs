using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGHelper.Base
{
    public class SVGUnSupported : SVGUnit
    {
        public SVGUnSupported(SVGWord doc, string sName) : base(doc)
        {
            m_sElementName = sName + ":unsupported";
            m_ElementType = SVGUnitType.typeUnsupported;
        }
    }
}
