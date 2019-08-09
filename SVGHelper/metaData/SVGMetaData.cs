using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SVGHelper.Base;

namespace SVGHelper.metaData
{
    public class SVGMetaData:SVGUnit
    {
       

        public SVGMetaData(SVGWord doc) : base(doc)
        {

            Init();

        }

        private void Init()
        {
            m_sElementName = "metaData";
            m_ElementType = SVGUnitType.metaData;
        }
    }
}
