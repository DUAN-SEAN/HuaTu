using SVGHelper.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGHelper
{
    public class SVGPolygon : SVGBaseShape
    {
        [Category("(Specific)")]
        [Description("The points that make up the polygon. All coordinate values are in the user coordinate system.")]
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

        /// <summary>
        /// It constructs a polygon element with no attribute.
        /// </summary>
        /// <param name="doc">SVG document.</param>
        public SVGPolygon(SVGWord doc) : base(doc)
        {
            Init();
        }

        /// <summary>
        /// It constructs a polygon element.
        /// </summary>
        /// <param name="doc">SVG document.</param>
        /// <param name="sPoints"></param>
        public SVGPolygon(SVGWord doc, string sPoints) : base(doc)
        {
            Init();

            Points = sPoints;
        }

        private void Init()
        {
            m_sElementName = "polygon";
            m_ElementType = SVGUnitType.typePolygon;

            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_Points, "");
        }
    }
}
