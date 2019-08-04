using SVGHelper.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGHelper
{
    [DefaultProperty("Id")]
    public class SVGUnit
    {
        /// <summary>
        /// 列出所有SVG元素类型。对于每个元素，库中都定义了一个特定的类。
        /// </summary>
        public enum SVGUnitType
        {
            typeUnsupported,
            typeSvg,
            typeDesc,
            typeText,
            typeGroup,
            typeRect,
            typeCircle,
            typeEllipse,
            typeLine ,
            typePath,
            typePolygon,
            typeImage,
            typePolyline,
            typeAnimate,
            typeAnimateColor,
            typeAnimateMotion,
            typeSet,
            devicePort,
            device
        };


        /// <summary>
        /// 用于为元素分配唯一名称的标准XML属性。
        /// </summary>
        [Category("(Core)")]
        [Description("Standard XML attribute for assigning a unique name to an element.")]
        public string Id
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrCore_Id);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrCore_Id, value);
            }
        }



        private class CEleComparer : IComparer
        {
            int IComparer.Compare(Object x, Object y)
            {
                SVGAttribute ax = (SVGAttribute)x;
                SVGAttribute ay = (SVGAttribute)y;

                if (ax.AttributeGroup == ay.AttributeGroup)
                {
                    if (ax.AttributeType < ay.AttributeType)
                    {
                        return -1;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else if (ax.AttributeGroup < ay.AttributeGroup)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
        }


        protected SVGUnit m_Parent;
        protected SVGUnit m_Child;
        protected SVGUnit m_Next;
        protected SVGUnit m_Previous;

        protected SVGWord m_doc;

        protected int m_nInternalId;
        protected string m_sElementName;
        protected string m_sElementValue;
        protected bool m_bHasValue;
        protected SVGUnitType m_ElementType;

        private ArrayList m_attributes;


        /// <summary>
        /// 它返回父元素。
        /// </summary>
        public SVGUnit getParent()
        {
            return m_Parent;
        }

        /// <summary>
        /// 它设置父元素。
        /// </summary>
        public void setParent(SVGUnit ele)
        {
            m_Parent = ele;
        }

        /// <summary>
        /// 它孕育了第一个子元素。
        /// </summary>
        public SVGUnit getChild()
        {
            return m_Child;
        }

        /// <summary>
        /// 它设置第一个子元素。
        /// </summary>
        public void setChild(SVGUnit ele)
        {
            m_Child = ele;
        }

        /// <summary>
        /// 它获取下一个同级元素。
        /// </summary>
        public SVGUnit getNext()
        {
            return m_Next;
        }

        /// <summary>
        /// 它设置下一个同级元素。
        /// </summary>
        public void setNext(SVGUnit ele)
        {
            m_Next = ele;
        }

        /// <summary>
        /// 它获取上一个同级元素。
        /// </summary>
        /// <returns>Previous element.</returns>
        public SVGUnit getPrevious()
        {
            return m_Previous;
        }

        /// <summary>
        /// 它设置前一个元素。
        /// </summary>
        /// <param name="ele">New previous element.</param>
        public void setPrevious(SVGUnit ele)
        {
            m_Previous = ele;
        }

        /// <summary>
        /// 它获取元素的内部ID。
        /// </summary>
        public int getInternalId()
        {
            return m_nInternalId;
        }

        /// <summary>
        /// 它设置元素的内部ID。
        /// </summary>
        public void setInternalId(int nId)
        {
            m_nInternalId = nId;
        }

        /// <summary>
        /// 它返回svg元素名。
        /// </summary>
        public string getElementName()
        {
            return m_sElementName;
        }

        /// <summary>
        /// 它返回当前元素值。
        /// </summary>
        public string getElementValue()
        {
            return m_sElementValue;
        }

        /// <summary>
        /// 设置元素值。
        /// </summary>
        public void setElementValue(string sValue)
        {
            m_sElementValue = sValue;
        }

        /// <summary>
        /// 指示SVG元素是否需要值的标志。
        /// </summary>
        public bool HasValue()
        {
            return m_bHasValue;
        }

        /// <summary>
        /// 它返回svg元素类型。
        /// </summary>
        /// <returns></returns>
        public SVGUnitType getElementType()
        {
            return m_ElementType;
        }

        /// <summary>
        /// 它返回从元素开始的SVG树的XML字符串。
        /// </summary>
        public string GetXML()
        {
            string sXML;

            sXML = OpenXMLTag();

            if (m_Child != null)
            {
                sXML += m_Child.GetXML();
            }

            sXML += CloseXMLTag();

            SVGUnit ele = m_Next;
            if (ele != null)
            {
                sXML += ele.GetXML();
            }

            SVGErr.Log("SvgElement", "GetXML", ElementInfo(), SVGErr._LogPriority.Info);

            return sXML;
        }

        /// <summary>
        /// 它返回svg元素的XML字符串。
        /// </summary>
        public string GetTagXml()
        {
            string sXML;

            sXML = OpenXMLTag();
            sXML += CloseXMLTag();

            return sXML;
        }

        /// <summary>
        /// 它获取所有元素属性。
        /// </summary>
        public void FillAttributeList(ArrayList aType, ArrayList aName, ArrayList aValue)
        {
            IComparer myComparer = new CEleComparer();
            m_attributes.Sort(myComparer);


            for (int i = 0; i < m_attributes.Count; i++)
            {
                SVGAttribute attr = (SVGAttribute)m_attributes[i];

                aType.Add(attr.AttributeType);
                aName.Add(attr.Name);
                aValue.Add(attr.Value);
            }
        }

        /// <summary>
        /// 它将元素eletoClone的所有属性复制到
        /// current element.
        /// </summary>
        public void CloneAttributeList(SVGUnit eleToClone)
        {
            ArrayList aType = new ArrayList();
            ArrayList aName = new ArrayList();
            ArrayList aValue = new ArrayList();

            eleToClone.FillAttributeList(aType, aName, aValue);

            m_attributes.Clear();


            for (int i = 0; i < aType.Count; i++)
            {
                AddAttr((SVGAttribute._SvgAttribute)aType[i], aValue[i]);
            }

            if (m_bHasValue)
            {
                m_sElementValue = eleToClone.m_sElementValue;
            }
        }

        /// <summary>
        /// 它返回一个字符串，其中包含用于日志记录的当前元素信息。
        /// </summary>
        public string ElementInfo()
        {
            string sMsg = "InternalId:" + m_nInternalId.ToString();

            if (m_Parent != null)
            {
                sMsg += " - Parent:" + m_Parent.getInternalId().ToString();
            }

            if (m_Previous != null)
            {
                sMsg += " - Previous:" + m_Previous.getInternalId().ToString();
            }

            if (m_Next != null)
            {
                sMsg += " - Next:" + m_Next.getInternalId().ToString();
            }

            if (m_Child != null)
            {
                sMsg += " - Child:" + m_Child.getInternalId().ToString();
            }

            return sMsg;
        }

        // ---------- PUBLIC METHODS END

        // ---------- PRIVATE METHODS

        protected SVGUnit(SVGWord doc)
        {
            SVGErr.Log("SvgElement", "SvgElement", "Element created", SVGErr._LogPriority.Info);

            m_doc = doc;

            m_attributes = new ArrayList();

            AddAttr(SVGAttribute._SvgAttribute.attrCore_Id, null);

            m_Parent = null;
            m_Child = null;
            m_Next = null;
            m_Previous = null;

            m_sElementName = "unsupported";
            m_sElementValue = "";
            m_bHasValue = false;
            m_ElementType = SVGUnitType.typeUnsupported;
        }

        ~SVGUnit()
        {
            SVGErr.Log("SvgElement", "SvgElement", "Element destroyed, InternalId:" + m_nInternalId.ToString(), SVGErr._LogPriority.Info);

            m_Parent = null;
            m_Child = null;
            m_Next = null;
            m_Previous = null;
        }

        protected string OpenXMLTag()
        {
            string sXML;

            sXML = "<" + m_sElementName;

            for (int i = 0; i < m_attributes.Count; i++)
            {
                SVGAttribute attr = (SVGAttribute)m_attributes[i];
                sXML += attr.GetXML();
            }

            if (m_sElementValue == "")
            {
                if (m_Child == null)
                {
                    sXML += " />\r\n";
                }
                else
                {
                    sXML += ">\r\n";
                }
            }
            else
            {
                sXML += ">";
                sXML += m_sElementValue;
            }

            return sXML;
        }

        protected string CloseXMLTag()
        {
            if ((m_sElementValue == "") && (m_Child == null))
            {
                return "";
            }
            else
            {
                return "</" + m_sElementName + ">\r\n";
            }
        }

        protected void AddAttr(SVGAttribute._SvgAttribute type, object objValue)
        {
            SVGAttribute attrToAdd = new SVGAttribute(type);
            attrToAdd.Value = objValue;

            m_attributes.Add(attrToAdd);
        }

        internal SVGAttribute GetAttribute(string sName)
        {
            for (int i = 0; i < m_attributes.Count; i++)
            {
                SVGAttribute attr = (SVGAttribute)m_attributes[i];
                if (attr.Name == sName)
                {
                    return attr;
                }
            }

            return null;
        }

        internal SVGAttribute GetAttribute(SVGAttribute._SvgAttribute type)
        {
            for (int i = 0; i < m_attributes.Count; i++)
            {
                SVGAttribute attr = (SVGAttribute)m_attributes[i];
                if (attr.AttributeType == type)
                {
                    return attr;
                }
            }

            return null;
        }
        internal bool SetAttributeValue(string sName, string sValue)
        {
            bool bReturn = false;

            for (int i = 0; i < m_attributes.Count; i++)
            {
                SVGAttribute attr = (SVGAttribute)m_attributes[i];
                if (attr.Name == sName)
                {
                    switch (attr.AttributeDataType)//获取属性的值所处类型，默认为字符串
                    {
                        case SVGAttribute._SvgAttributeDataType.datatypeString://跳到下一个case执行

                        case SVGAttribute._SvgAttributeDataType.datatypeHRef:
                            attr.Value = sValue;
                            if (attr.AttributeType == SVGAttribute._SvgAttribute.attrStyle_Style)
                                ParseStyle(sValue);
                            break;

                        case SVGAttribute._SvgAttributeDataType.datatypeEnum:
                            int nValue = 0;
                            try
                            {
                                nValue = Convert.ToInt32(sValue);
                            }
                            catch
                            {
                            }

                            attr.Value = nValue;
                            break;

                        case SVGAttribute._SvgAttributeDataType.datatypeColor:

                            if (sValue == "")
                            {
                                attr.Value = Color.Transparent;
                            }
                            else
                            {
                                Color c = attr.String2Color(sValue);
                                attr.Value = c;
                            }
                            break;
                    }

                    bReturn = true;

                    break;
                }
            }

            return bReturn;
        }

        internal bool SetAttributeValue(SVGAttribute._SvgAttribute type, object objValue)
        {
            bool bReturn = false;

            for (int i = 0; i < m_attributes.Count; i++)
            {
                SVGAttribute attr = (SVGAttribute)m_attributes[i];
                if (attr.AttributeType == type)
                {
                    bReturn = true;
                    attr.Value = objValue;

                    break;
                }
            }

            return bReturn;
        }

        internal bool GetAttributeValue(SVGAttribute._SvgAttribute type, out object objValue)
        {
            bool bReturn = false;
            objValue = null;

            for (int i = 0; i < m_attributes.Count; i++)
            {
                SVGAttribute attr = (SVGAttribute)m_attributes[i];
                if (attr.AttributeType == type)
                {
                    bReturn = true;
                    objValue = attr.Value;

                    break;
                }
            }

            return bReturn;
        }

        internal object GetAttributeValue(SVGAttribute._SvgAttribute type)
        {
            object objValue;

            if (GetAttributeValue(type, out objValue))
            {
                return objValue;
            }
            else
            {
                return null;
            }
        }

        internal string GetAttributeStringValue(SVGAttribute._SvgAttribute type)
        {
            object objValue = GetAttributeValue(type);

            if (objValue != null)
            {
                return objValue.ToString();
            }
            else
            {
                return "";
            }
        }

        internal int GetAttributeIntValue(SVGAttribute._SvgAttribute type)
        {
            object objValue = GetAttributeValue(type);

            if (objValue != null)
            {
                int nValue = 0;
                try
                {
                    nValue = Convert.ToInt32(objValue.ToString());
                }
                catch
                {
                }

                return nValue;
            }
            else
            {
                return 0;
            }
        }

        internal Color GetAttributeColorValue(SVGAttribute._SvgAttribute type)
        {
            object objValue = GetAttributeValue(type);

            if (objValue != null)
            {
                Color cValue = Color.Black;
                try
                {
                    cValue = (Color)(objValue);
                }
                catch
                {
                }

                return cValue;
            }
            else
            {
                return Color.Black;
            }
        }

        // ---------- PRIVATE METHODS END
        public virtual void ParseStyle(string sval)
        {
        }
    }
}
