using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using SVGHelper.Fix;

namespace SVGHelper.Base
{
    public class SVGWord
    {


        // 文档的根目录
        private SVGRoot m_root;

        // 文档元素，哈希表键是InternalID
        private Hashtable m_elements;

        // 存储要分配给新元素的下一个InternalID
        private int m_nNextId;

        private string m_sXmlDeclaration;
        private string m_sXmlDocType;


        /// <summary>
        /// 构造器
        /// </summary>
        public SVGWord()
        {
            m_root = null;
            m_nNextId = 1;
            m_elements = new Hashtable();
        }

        /// <summary>
        /// 它创建了一个新的空SVG文档，其中只包含根元素。
        /// 如果当前文档存在，则会将其销毁。
        /// </summary>
        public SVGRoot CreateNewDocument()
        {
            if (m_root != null)
            {
                m_root = null;
                m_nNextId = 1;
                m_elements.Clear();
            }

            m_root = new SVGRoot(this);
            m_root.setInternalId(m_nNextId++);

            m_elements.Add(m_root.getInternalId(), m_root);

            m_sXmlDeclaration = "<?xml version=\"1.0\" standalone=\"no\"?>";
            m_sXmlDocType =
                "<!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.1//EN\" \"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\">";

            m_root.SetAttributeValue(SVGAttribute._SvgAttribute.attrSvg_XmlNs, "http://www.w3.org/2000/svg");
            m_root.SetAttributeValue(SVGAttribute._SvgAttribute.attrSvg_Version, "1.1");

            return m_root;
        }

        public bool LoadFromFile(string sFilename)
        {
            SVGErr err = new SVGErr("SvgDoc", "LoadFromFile");
            bool bResult = true;
            try
            {
                XmlTextReader reader;
                reader = new XmlTextReader(sFilename);
                if (!LoadFromFile(reader))
                    MessageBox.Show("Error reading Svg document", "SvgPaint");
            }
            catch (Exception e)
            {
                err.LogException(e);
                bResult = false;
            }

            return bResult;
        }

        /// <summary>
        /// 它创建一个从文件读取的SVG文档。
        /// 如果当前文档存在，则会将其销毁。
        /// </summary>
        public bool LoadFromFile(XmlTextReader reader)
        {
            SVGErr err = new SVGErr("SvgDoc", "LoadFromFile");
            err.LogParameter("sFilename", reader.BaseURI);

            if (m_root != null)
            {
                m_root = null;
                m_nNextId = 1;
                m_elements.Clear();
            }

            bool bResult = true;

            try
            {
                //				XmlTextReader reader;
                //				reader = new XmlTextReader(sFilename);
                reader.WhitespaceHandling = WhitespaceHandling.None;
                reader.Normalization = false;
                reader.XmlResolver = null;
                reader.Namespaces = false;

                string tmp;
                SVGUnit eleParent = null;
                SVGUnit eleLast = null;

                try
                {
                    // 分析文件并显示每个节点。
                    while (reader.Read() && bResult)
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Attribute:
                                tmp = reader.Name;
                                tmp = reader.Value;
                                break;

                            case XmlNodeType.Element://找到一个标签
                            {
                                SVGUnit ele = AddElement(eleParent, reader.Name, ref eleLast);

                                if (ele == null)
                                {
                                    err.Log("Svg element cannot be added. Name: " + reader.Name,
                                        SVGErr._LogPriority.Warning);
                                    bResult = false;
                                }
                                else
                                {
                                    eleParent = ele;

                                    if (reader.IsEmptyElement)
                                    {
                                        if (eleParent != null)
                                        {
                                            eleParent = eleParent.getParent();
                                        }
                                    }

                                    bool bLoop = reader.MoveToFirstAttribute();
                                    while (bLoop)//直到读完属性为止
                                    {
                                        if (!ele.SetAttributeValue(reader.Name, reader.Value))
                                        {
                                            err.Log("Read AttributeValue : " + reader.Value, SVGErr._LogPriority.Warning);
                                        }

                                        bLoop = reader.MoveToNextAttribute();
                                    }
                                }
                            }
                                break;

                            case XmlNodeType.Text:
                                if (eleParent != null)
                                {
                                    eleParent.setElementValue(reader.Value);
                                }

                                break;

                            case XmlNodeType.CDATA:

                                err.Log("Unexpected item: " + reader.Value, SVGErr._LogPriority.Warning);
                                break;

                            case XmlNodeType.ProcessingInstruction:

                                err.Log("Unexpected item: " + reader.Value, SVGErr._LogPriority.Warning);
                                break;

                            case XmlNodeType.Comment:

                                err.Log("Unexpected item: " + reader.Value, SVGErr._LogPriority.Warning);
                                break;

                            case XmlNodeType.XmlDeclaration:
                                m_sXmlDeclaration = "<?xml " + reader.Value + "?>";
                                break;

                            case XmlNodeType.Document:
                                err.Log("Unexpected item: " + reader.Value, SVGErr._LogPriority.Warning);
                                break;

                            case XmlNodeType.DocumentType:
                            {
                                string sDTD1;
                                string sDTD2;

                                sDTD1 = reader.GetAttribute("PUBLIC");
                                sDTD2 = reader.GetAttribute("SYSTEM");

                                m_sXmlDocType = "<!DOCTYPE svg PUBLIC \"" + sDTD1 + "\" \"" + sDTD2 + "\">";
                            }
                                break;

                            case XmlNodeType.EntityReference:
                                err.Log("Unexpected item: " + reader.Value, SVGErr._LogPriority.Warning);
                                break;

                            case XmlNodeType.EndElement:
                                if (eleParent != null)
                                {

                                    eleParent = eleParent.getParent();
                                    eleLast = null;
                                    if (eleParent != null)
                                    {
                                        var templast = eleParent.getChild();
                                        while (templast != null)
                                        {
                                            eleLast = templast;
                                            templast = templast.getNext();
                                        }
                                    }
                                }

                                break;
                        } // switch
                    } // while
                } // read try
                catch (XmlException xmle)
                {
                    err.LogException(xmle);
                    err.LogParameter("Line Number", xmle.LineNumber.ToString());
                    err.LogParameter("Line Position", xmle.LinePosition.ToString());

                    bResult = false;
                }
                catch (Exception e)
                {
                    err.LogException(e);
                    bResult = false;
                }
                finally
                {
                    reader.Close();
                }
            }
            catch
            {
                err.LogUnhandledException();
                bResult = false;
            }

            err.LogEnd(bResult);

            return bResult;
        }

        /// <summary>
        /// 它将当前SVG文档保存到一个文件中。
        /// </summary>
        public bool SaveToFile(string sFilename)
        {
            SVGErr err = new SVGErr("SvgDoc", "SaveToFile");
            err.LogParameter("sFilename", sFilename);

            bool bResult = false;
            StreamWriter sw = null;
            try
            {
                sw = File.CreateText(sFilename);
                bResult = true;
            }
            catch (UnauthorizedAccessException uae)
            {
                err.LogException(uae);
            }
            catch (DirectoryNotFoundException dnfe)
            {
                err.LogException(dnfe);
            }
            catch (ArgumentException ae)
            {
                err.LogException(ae);
            }
            catch
            {
                err.LogUnhandledException();
            }

            if (!bResult)
            {
                err.LogEnd(false);

                return false;
            }

            try
            {
                sw.Write(GetXML());
                sw.Close();
            }
            catch
            {
                err.LogUnhandledException();
                err.LogEnd(false);

                return false;
            }

            err.LogEnd(true);

            return true;
        }

        /// <summary>
        /// 它返回整个SVG文档的XML字符串。
        /// </summary>
        public string GetXML()
        {
            if (m_root == null)
            {
                return "";
            }

            string sXML;

            sXML = m_sXmlDeclaration + "\r\n";
            sXML += m_sXmlDocType;
            sXML += "\r\n";

            sXML += m_root.GetXML();

            return sXML;
        }

        /// <summary>
        /// 它返回具有给定内部（数字）标识符的svgelement。
        /// </summary>
        public SVGUnit GetSvgElement(int nInternalId)
        {
            if (!m_elements.ContainsKey(nInternalId))
            {
                return null;
            }

            return (SVGUnit) m_elements[nInternalId];
        }

        /// <summary>
        /// It returns the root element of the SVG document.
        /// </summary>
        /// <returns>
        /// Root element.
        /// </returns>
        public SVGRoot GetSvgRoot()
        {
            return m_root;
        }

        /// <summary>
        /// 它返回具有给定XML ID的svgelement。
        /// </summary>
        public SVGUnit GetSvgElement(string sId)
        {
            SVGUnit eleToReturn = null;

            IDictionaryEnumerator e = m_elements.GetEnumerator();

            bool bLoop = e.MoveNext();
            while (bLoop)
            {
                string sValue = "";

                SVGUnit ele = (SVGUnit) e.Value;
                sValue = ele.GetAttributeStringValue(SVGAttribute._SvgAttribute.attrCore_Id);
                if (sValue == sId)
                {
                    eleToReturn = ele;
                    bLoop = false;
                }

                bLoop = e.MoveNext();
            }

            return eleToReturn;
        }

        /// <summary>
        /// 它将新元素eletoadd添加为给定父元素的最后一个子元素。
        /// </summary>
        public void AddElement(SVGUnit parent, SVGUnit eleToAdd, ref SVGUnit last)
        {
            SVGErr err = new SVGErr("SvgDoc", "AddElement");

            if (eleToAdd == null || m_root == null)
            {
                err.LogEnd(false);
                return;
            }

            SVGUnit parentToAdd = m_root;
            if (parent != null)
            {
                parentToAdd = parent;
            }

            eleToAdd.setInternalId(m_nNextId++);
            m_elements.Add(eleToAdd.getInternalId(), eleToAdd);

            eleToAdd.setParent(parentToAdd);
            if (parentToAdd.getChild() == null)
            {
                // 元素是第一个子元素
                parentToAdd.setChild(eleToAdd);
                last = eleToAdd;
            }
            else
            {
                // 将元素添加为最后一个同级
                //SvgElement last = GetLastSibling(parentToAdd.getChild());

                if (last != null)
                {
                    last.setNext(eleToAdd);
                    eleToAdd.setPrevious(last);
                    last = eleToAdd;
                }
                else
                {
                    last = parentToAdd.getChild();
                }
            }

            err.Log(eleToAdd.ElementInfo(), SVGErr._LogPriority.Info);
            err.LogEnd(true);
        }

        /// <summary>
        /// 它根据提供的元素名称创建一个新元素
        /// 并将新元素添加为给定父元素的最后一个子元素。
        /// </summary>
        public SVGUnit AddElement(SVGUnit parent, string sName, ref SVGUnit last)
        {
            SVGUnit eleToReturn = null;

            if (sName == "svg")
            {
                m_root = new SVGRoot(this);
                m_root.setInternalId(m_nNextId++);

                m_elements.Add(m_root.getInternalId(), m_root);
                eleToReturn = m_root;
            }
            else if (sName == "desc")
            {
                eleToReturn = AddDesc(parent, ref last);
            }
            else if (sName == "text")
            {
                eleToReturn = AddText(parent, ref last);
            }
            else if (sName == "g")//检查元素添加一个组
            {
                eleToReturn = AddGroup(parent, ref last);
            }
            else if (sName == "rect")
            {
                eleToReturn = AddRect(parent, ref last);
            }
            else if (sName == "circle")
            {
                eleToReturn = AddCircle(parent, ref last);
            }
            else if (sName == "ellipse")
            {
                eleToReturn = AddEllipse(parent, ref last);
            }
            else if (sName == "line")
            {
                eleToReturn = AddLine(parent, ref last);
            }
            else if (sName == "path")
            {
                eleToReturn = AddPath(parent, ref last);
            }
            else if (sName == "polygon")
            {
                eleToReturn = AddPolygon(parent, ref last);
            }
            else if (sName == "image")
            {
                eleToReturn = AddImage(parent, ref last);
            }
            else if (sName == "polyline")
            {
                eleToReturn = AddPolyline(parent, ref last);
            }
            else if (sName == "animate")
            {
                eleToReturn = AddAnimate(parent, ref last);
            }
            else if (sName == "devicePort")
            {
                eleToReturn = AddDevicePort(parent, ref last);
            }else if (sName == "defs")
            {
                eleToReturn = AddDefs(parent, ref last);
            }else if (sName == "symbol")
            {
                eleToReturn = AddSymbols(parent, ref last);
            }else if (sName == "use")
            {
                eleToReturn = AddUse(parent, ref last);
            }
            else
            {
                if (parent != null)
                {
                    eleToReturn = AddUnsupported(parent, sName, ref last);
                }
            }

            return eleToReturn;
        }

        private SVGUnit AddUse(SVGUnit parent, ref SVGUnit last)
        {
            SVGUse svgUse = new SVGUse(this);
            AddElement(parent, svgUse, ref last);
            return svgUse;

        }

        private SVGUnit AddSymbols(SVGUnit parent, ref SVGUnit last)
        {
            SVGSymbol svgSymbol = new SVGSymbol(this);
            AddElement(parent, svgSymbol, ref last);
            return svgSymbol;

        }

        private SVGUnit AddDefs(SVGUnit parent, ref SVGUnit last)
        {
            SVGDef svgDef = new SVGDef(this);
            AddElement(parent, svgDef, ref last);
            return svgDef;

        }

        private SVGUnit AddDevicePort(SVGUnit parent, ref SVGUnit last)
        {
            SVGDevicePort port = new SVGDevicePort(this);

            AddElement(parent, port, ref last);

            return port;
        }

        /// <summary>
        /// 它创建了一个新的元素，从eletoclone复制所有属性；新的
        /// 元素插入到提供的父元素下。
        /// </summary>
        public SVGUnit CloneElement(SVGUnit parent, SVGUnit eleToClone, SVGUnit last)
        {
            string sOldId = eleToClone.GetAttributeStringValue(SVGAttribute._SvgAttribute.attrCore_Id);
            string sNewId = sOldId;

            if (sOldId != "")
            {
                int i = 1;

                while (GetSvgElement(sNewId) != null)
                {
                    sNewId = sOldId + "_" + i.ToString();
                    i++;
                }
            }

            SVGUnit eleNew = AddElement(parent, eleToClone.getElementName(), ref last);
            eleNew.CloneAttributeList(eleToClone);

            if (sNewId != "")
            {
                eleNew.SetAttributeValue(SVGAttribute._SvgAttribute.attrCore_Id, sNewId);
            }

            if (eleToClone.getChild() != null)
            {
                eleNew.setChild(CloneElement(eleNew, eleToClone.getChild(), last));

                if (eleToClone.getChild().getNext() != null)
                {
                    eleNew.getChild().setNext(CloneElement(eleNew, eleToClone.getChild().getNext(), last));
                }
            }

            return eleNew;
        }

        /// <summary>
        /// 它创建了一个新的不支持SVG的元素。
        /// </summary>
        public SVGUnSupported AddUnsupported(SVGUnit parent, string sName, ref SVGUnit last)
        {
            SVGUnSupported uns = new SVGUnSupported(this, sName);

            AddElement(parent, uns, ref last);

            return uns;
        }

        /// <summary>
        /// 它创建了一个新的svg desc元素。
        /// </summary>
        public SVGDesc AddDesc(SVGUnit parent, ref SVGUnit last)
        {
            SVGDesc desc = new SVGDesc(this);

            AddElement(parent, desc, ref last);

            return desc;
        }

        /// <summary>
        /// 它创建了一个新的svg group元素。
        /// </summary>
        public SVGGroup AddGroup(SVGUnit parent, ref SVGUnit last)
        {
            SVGGroup grp = new SVGGroup(this);

            AddElement(parent, grp, ref last);

            return grp;
        }

        /// <summary>
        /// 它创建一个新的SVG文本元素。
        /// </summary>
        public SVGText AddText(SVGUnit parent, ref SVGUnit last)
        {
            SVGText txt = new SVGText(this);

            AddElement(parent, txt, ref last);

            return txt;
        }

        /// <summary>
        /// 它创建一个新的svg rect元素。
        /// </summary>
        public SVGRect AddRect(SVGUnit parent, ref SVGUnit last)
        {
            SVGRect rect = new SVGRect(this);

            AddElement(parent, rect, ref last);

            return rect;
        }

        /// <summary>
        /// 它创建了一个新的svg circle元素。
        /// </summary>
        public SVGCircle AddCircle(SVGUnit parent, ref SVGUnit last)
        {
            SVGCircle circle = new SVGCircle(this);

            AddElement(parent, circle, ref last);

            return circle;
        }

        /// <summary>
        /// 它创建了一个新的SVG椭圆元素。
        /// </summary>
        public SVGEllipse AddEllipse(SVGUnit parent, ref SVGUnit last)
        {
            SVGEllipse ellipse = new SVGEllipse(this);

            AddElement(parent, ellipse, ref last);

            return ellipse;
        }

        /// <summary>
        /// 它创建了一个新的SVG行元素。
        /// </summary>
        public SVGLine AddLine(SVGUnit parent, ref SVGUnit last)
        {
            SVGLine line = new SVGLine(this);

            AddElement(parent, line, ref last);

            return line;
        }

        /// <summary>
        /// 它创建了一个新的SVG路径元素。
        /// </summary>
        public SVGPath AddPath(SVGUnit parent, ref SVGUnit last)
        {
            SVGPath path = new SVGPath(this);

            AddElement(parent, path, ref last);

            return path;
        }

        /// <summary>
        /// 它创建一个新的SVG多边形元素。
        /// </summary>
        public SVGPolygon AddPolygon(SVGUnit parent, ref SVGUnit last)
        {
            SVGPolygon poly = new SVGPolygon(this);

            AddElement(parent, poly, ref last);

            return poly;
        }

        public SVGAnimate AddAnimate(SVGUnit parent, ref SVGUnit last)
        {
            SVGAnimate svgAnimate = new SVGAnimate(this);

            AddElement(parent, svgAnimate, ref last);

            return svgAnimate;
        }



        /// <summary>
        /// 它创建一个新的SVG折线元素。
        /// </summary>
        public SVGPolyline AddPolyline(SVGUnit parent, ref SVGUnit last)
        {
            SVGPolyline poly = new SVGPolyline(this);

            AddElement(parent, poly, ref last);

            return poly;
        }

        /// <summary>
        /// 它创建了一个新的SVG图像元素。
        /// </summary>
        public SVGImage AddImage(SVGUnit parent, ref SVGUnit last)
        {
            SVGImage img = new SVGImage(this);

            AddElement(parent, img, ref last);

            return img;
        }

        /// <summary>
        /// 它从文档中删除一个元素。
        /// </summary>
        public bool DeleteElement(SVGUnit ele)
        {
            return DeleteElement(ele, true);
        }

        /// <summary>
        /// 它从文档中删除一个元素。
        /// </summary>
        public bool DeleteElement(int nInternalId)
        {
            return DeleteElement(GetSvgElement(nInternalId), true);
        }

        /// <summary>
        /// 它从文档中删除一个元素。
        /// </summary>
        public bool DeleteElement(string sId)
        {
            return DeleteElement(GetSvgElement(sId), true);
        }

        /// <summary>
        /// 它将元素移动到当前的上一个同级之前。
        /// </summary>
        public bool ElementPositionUp(SVGUnit ele)
        {
            SVGErr err = new SVGErr("SvgDoc", "ElementPositionUp");
            err.Log("Element to move " + ele.ElementInfo(), SVGErr._LogPriority.Info);

            SVGUnit parent = ele.getParent();
            if (parent == null)
            {
                err.Log("Root node cannot be moved", SVGErr._LogPriority.Info);
                err.LogEnd(false);

                return false;
            }

            if (IsFirstChild(ele))
            {
                err.Log("Element is already at the first position", SVGErr._LogPriority.Info);
                err.LogEnd(false);

                return false;
            }

            SVGUnit nxt = ele.getNext();
            SVGUnit prv = ele.getPrevious();
            SVGUnit prv2 = null;

            ele.setNext(null);
            ele.setPrevious(null);

            if (nxt != null)
            {
                nxt.setPrevious(prv);
            }

            if (prv != null)
            {
                prv.setNext(nxt);
                prv2 = prv.getPrevious();
                prv.setPrevious(ele);

                if (IsFirstChild(prv))
                {
                    if (prv.getParent() != null)
                    {
                        prv.getParent().setChild(ele);
                    }
                }
            }

            if (prv2 != null)
            {
                prv2.setNext(ele);
            }

            ele.setNext(prv);
            ele.setPrevious(prv2);

            err.Log("Element moved " + ele.ElementInfo(), SVGErr._LogPriority.Info);
            err.LogEnd(true);

            return true;
        }

        /// <summary>
        /// 它将元素在树层次结构中向上移动一级。
        /// </summary>
        public bool ElementLevelUp(SVGUnit ele)
        {
            SVGErr err = new SVGErr("SvgDoc", "ElementLevelUp");
            err.Log("Element to move " + ele.ElementInfo(), SVGErr._LogPriority.Info);

            SVGUnit parent = ele.getParent();
            if (parent == null)
            {
                err.Log("Root node cannot be moved", SVGErr._LogPriority.Info);
                err.LogEnd(false);

                return false;
            }

            if (parent.getParent() == null)
            {
                err.Log("An element cannot be moved up to the root", SVGErr._LogPriority.Info);
                err.LogEnd(false);

                return false;
            }

            SVGUnit nxt = ele.getNext();

            // the first child of the parent became the next
            parent.setChild(nxt);

            if (nxt != null)
            {
                nxt.setPrevious(null);
            }

            // get the last sibling of the parent
            SVGUnit last = GetLastSibling(parent);
            if (last != null)
            {
                last.setNext(ele);
            }

            ele.setParent(parent.getParent());
            ele.setPrevious(last);
            ele.setNext(null);

            return true;
        }

        /// <summary>
        /// It moves the element after its current next sibling.
        /// </summary>
        public bool ElementPositionDown(SVGUnit ele)
        {
            SVGErr err = new SVGErr("SvgDoc", "ElementPositionDown");
            err.Log("Element to move " + ele.ElementInfo(), SVGErr._LogPriority.Info);

            SVGUnit parent = ele.getParent();
            if (parent == null)
            {
                err.Log("Root node cannot be moved", SVGErr._LogPriority.Info);
                err.LogEnd(false);

                return false;
            }

            if (IsLastSibling(ele))
            {
                err.Log("Element is already at the last sibling position", SVGErr._LogPriority.Info);
                err.LogEnd(false);

                return false;
            }

            SVGUnit nxt = ele.getNext();
            SVGUnit nxt2 = null;
            SVGUnit prv = ele.getPrevious();

            // fix Next
            if (nxt != null)
            {
                nxt.setPrevious(ele.getPrevious());
                nxt2 = nxt.getNext();
                nxt.setNext(ele);
            }

            // fix Previous
            if (prv != null)
            {
                prv.setNext(nxt);
            }

            // fix Element
            if (IsFirstChild(ele))
            {
                parent.setChild(nxt);
            }

            ele.setPrevious(nxt);
            ele.setNext(nxt2);

            if (nxt2 != null)
            {
                nxt2.setPrevious(ele);
            }

            err.Log("Element moved " + ele.ElementInfo(), SVGErr._LogPriority.Info);
            err.LogEnd(true);

            return true;
        }


        private bool DeleteElement(SVGUnit ele, bool bDeleteFromParent)
        {
            SVGErr err = new SVGErr("SvgDoc", "DeleteElement");

            if (ele == null)
            {
                err.LogEnd(false);

                return false;
            }

            SVGUnit parent = ele.getParent();
            if (parent == null)
            {
                err.Log("root node cannot be delete!", SVGErr._LogPriority.Info);
                err.LogEnd(false);

                return false;
            }

            if (ele.getPrevious() != null)
            {
                ele.getPrevious().setNext(ele.getNext());
            }

            if (ele.getNext() != null)
            {
                ele.getNext().setPrevious(ele.getPrevious());
            }

            if (bDeleteFromParent)
            {
                if (IsFirstChild(ele))
                {
                    ele.getParent().setChild(ele.getNext());
                }
            }

            SVGUnit child = ele.getChild();

            while (child != null)
            {
                DeleteElement(child, false);
                child = child.getNext();
            }

            m_elements.Remove(ele.getInternalId());

            err.Log(ele.ElementInfo(), SVGErr._LogPriority.Info);
            err.LogEnd(true);

            return true;
        }

        private bool IsFirstChild(SVGUnit ele)
        {
            if (ele.getParent() == null)
            {
                return false;
            }

            if (ele.getParent().getChild() == null)
            {
                return false;
            }

            return (ele.getInternalId() == ele.getParent().getChild().getInternalId());
        }

        private bool IsLastSibling(SVGUnit ele)
        {
            SVGUnit last = GetLastSibling(ele);

            if (last == null)
            {
                return false;
            }

            return (ele.getInternalId() == last.getInternalId());
        }

        private SVGUnit GetLastSibling(SVGUnit ele)
        {
            if (ele == null)
            {
                return null;
            }

            SVGUnit last = ele;
            while (last.getNext() != null)
            {
                last = last.getNext();
            }

            return last;
        }
    }
}
