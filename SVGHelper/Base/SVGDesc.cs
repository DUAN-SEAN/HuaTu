using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGHelper.Base
{
    /// <summary>
    /// 它表示desc svg元素。
    /// SVG图形中的每个容器元素或图形元素都可以提供
    /// 说明仅为文本的“desc”和/或“title”说明字符串。
    /// </summary>
    public class SVGDesc : SVGUnit
    {
        /// <summary>
        /// 元素的值。
        /// </summary>
        [Category("(Specific)")]
        [Description("The value of the element.")]
        public string Value
        {
            get
            {
                return m_sElementValue;
            }

            set
            {
                m_sElementValue = value;
            }
        }

        /// <summary>
        /// 它构造一个没有属性的desc元素。
        /// </summary>
        /// <param name="doc">SVG document.</param>
        public SVGDesc(SVGWord doc) : base(doc)
        {
            Init();
        }

        /// <summary>
        /// 构造一个desc元素。
        /// </summary>
        /// <param name="doc">SVG document.</param>
        /// <param name="sValue"></param>
        public SVGDesc(SVGWord doc, string sValue) : base(doc)
        {
            Init();

            Value = sValue;
        }

        private void Init()
        {
            m_sElementName = "desc";
            m_bHasValue = true;
            m_ElementType = SVGUnitType.typeDesc;
        }
    }
}
