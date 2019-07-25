using SVGHelper.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGHelper
{
    public class SVGPath : SVGBaseShape
    {
        [Category("(Specific)")]
        [Description("The definition of the outline of a shape.")]
        public string PathData
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrSpecific_PathData);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrSpecific_PathData, value);
            }
        }

        /// <summary>
        /// The author's computation of the total length of the path, in user units.
        /// </summary>
        [Category("(Specific)")]
        [Description("The author's computation of the total length of the path, in user units.")]
        public string PathLength
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrSpecific_PathLength);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrSpecific_PathLength, value);
            }
        }

        /// <summary>
        /// It constructs a path element with no attribute.
        /// </summary>
        public SVGPath(SVGWord doc) : base(doc)
        {
            Init();
        }

        /// <summary>
        /// It constructs a path element.
        /// </summary>
        public SVGPath(SVGWord doc, string sPathData) : base(doc)
        {
            Init();

            PathData = sPathData;
        }

        private void Init()
        {
            m_sElementName = "path";
            m_ElementType = SVGUnitType.typePath;

            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_PathData, "");
            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_PathLength, "");
            AddAttr(SVGAttribute._SvgAttribute.attrSpecific_ShapeName, "");
        }
    }
}
