using SVGHelper.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGHelper
{
    public class SVGPolyline : SVGBaseShape
    {
        [Category("(Specific)")]
        [Description("The points that make up the polyline. All coordinate values are in the user coordinate system.")]
        public string Points
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrSpecific_Points);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrSpecific_Points, value);
            }
        }

        public SVGPolyline(SVGWord doc) : base(doc)
        {
            Init();
        }

        public SVGPolyline(SVGWord doc, string sPoints) : base(doc)
        {
            Init();

            Points = sPoints;
        }

        private void Init()
        {
            m_sElementName = "polygon";
            m_ElementType = SVGUnitType.typePolyline;

            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_Points, "");
        }
    }
}
