using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SVGHelper.Base;

namespace SVGHelper.Fix
{
    public class SVGDef:SVGUnit
    {
        public SVGDef(SVGWord doc) : base(doc)
        {
            Init();
        }

        private void Init()
        {
            m_sElementName = "defs";
            m_ElementType = SVGUnitType.def;



        }
    }
}
