using DrawWork.Command;
using SVGHelper;
using SVGHelper.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrawWork
{
    [Serializable]
    public class DrawObjectList
    {
        #region 字段

        private readonly ArrayList _graphicsList;
        private readonly ArrayList _inMemoryList;
        private readonly UndoRedo _undoRedo;

        private bool _isCut;

        #endregion 字段

        #region 构造器

        public DrawObjectList()
        {
            _graphicsList = new ArrayList();
            _inMemoryList = new ArrayList();
            _undoRedo = new UndoRedo();
        }

        #endregion 构造器

        #region 属性

        /// <summary>
        /// Count和此[Nindex]允许读取所有图形对象
        ///从循环中的graphicslist。
        /// </summary>
        public int Count
        {
            get
            {
                return _graphicsList.Count;
            }
        }

        /// <summary>
        /// 获取或设置描述
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// SelectedCount and GetSelectedObject allow to read
        /// selected objects in the loop
        /// </summary>
        public int SelectionCount
        {
            get
            {
                int n = 0;
                foreach (DrawObject o in _graphicsList)
                {
                    if (o.Selected)
                        n++;
                }
                return n;
            }
        }

        #endregion Properties

        #region Indexers

        public DrawObject this[int index]
        {
            get
            {
                if (index < 0 || index >= _graphicsList.Count)
                    return null;

                return ((DrawObject)_graphicsList[index]);
            }
        }

        #endregion Indexers

        #region Methods

        public void Add(DrawObject obj)
        {
            // insert to the top of z-order
            _graphicsList.Insert(0, obj);
            var create = new CreateCommand(obj, _graphicsList);
            _undoRedo.AddCommand(create);//添加到回退
        }

        //将读取的svg根传入 扫描每一个unit
        public void AddFromSvg(SVGUnit ele)
        {
            while (ele != null)
            {
                DrawObject o = CreateDrawObject(ele);
                if (o != null)
                    Add(o);
                 SVGUnit child = ele.getChild();
                while (child != null)
                {
                    AddFromSvg(child);
                    child = child.getNext();
                }
                ele = ele.getNext();
            }
        }

        public bool AreItemsInMemory()
        {
            return (_inMemoryList.Count > 0);
        }

       
        public bool Clear()
        {
            bool result = (_graphicsList.Count > 0);
            _graphicsList.Clear();
            return result;
        }

       
        public void CutSelection()
        {
            int i;
            int n = _graphicsList.Count;
            _inMemoryList.Clear();
            for (i = n - 1; i >= 0; i--)
            {
                if (((DrawObject)_graphicsList[i]).Selected)
                {
                    _inMemoryList.Add(_graphicsList[i]);
                }
            }
            _isCut = true;

            var cmd = new CutCommand(_graphicsList, _inMemoryList);
            cmd.Execute();
            _undoRedo.AddCommand(cmd);
        }

   
        public bool CopySelection()
        {
            bool result = false;
            int n = _graphicsList.Count;

            for (int i = n - 1; i >= 0; i--)
            {
                if (((DrawObject)_graphicsList[i]).Selected)
                {
                    _inMemoryList.Clear();
                    _inMemoryList.Add(_graphicsList[i]);
                    result = true;
                    _isCut = false;
                }
            }

            return result;
        }


   
        public void PasteSelection()
        {
            int n = _inMemoryList.Count;

            UnselectAll();

            if (n > 0)
            {
                var tempList = new ArrayList();

                int i;
                for (i = n - 1; i >= 0; i--)
                {
                    tempList.Add(((DrawObject)_inMemoryList[i]).Clone());
                }

                if (_inMemoryList.Count > 0)
                {
                    var cmd = new PasteCommand(_graphicsList, tempList);
                    cmd.Execute();
                    _undoRedo.AddCommand(cmd);

                    //If the items are cut, we will not delete it
                    if (_isCut)
                        _inMemoryList.Clear();
                }
            }
        }

        /// <summary>
        /// 删除所选项
        /// </summary>
        public bool DeleteSelection()
        {
            var cmd = new DeleteCommand(_graphicsList);
            cmd.Execute();
            _undoRedo.AddCommand(cmd);
            return true;
        }

        public void Draw(Graphics g)
        {
            int n = _graphicsList.Count;
            DrawObject o;

            for (int i = n - 1; i >= 0; i--)
            //for (int i = 0; i < graphicsList.Count; i++ )
            {
                o = (DrawObject)_graphicsList[i];

                o.Draw(g);

                if (o.Selected)
                {
                    o.DrawTracker(g);
                    o.DrawRotaryKnob(g);
                }
            }
        }

        public List<DrawObject> GetAllSelected()
        {
            var selectionList = new List<DrawObject>();
            foreach (DrawObject o in _graphicsList)
            {
                if (o.Selected)
                    selectionList.Add(o);
            }
            return selectionList;
        }

        public DrawObject GetFirstSelected()
        {
            foreach (DrawObject o in _graphicsList)
            {
                if (o.Selected)
                    return o;
            }
            return null;
        }

        public DrawObject GetSelectedObject(int index)
        {
            int n = -1;
            foreach (DrawObject o in _graphicsList)
            {
                if (o.Selected)
                {
                    n++;

                    if (n == index)
                        return o;
                }
            }
            return null;
        }

        /// <summary>
        /// 将对象保存到序列化流
        /// </summary>
        //[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter=true)]
        public string GetXmlString(SizeF scale)
        {
            string sXml = "";
            int n = _graphicsList.Count;
            for (int i = n - 1; i >= 0; i--)
            {
                sXml += ((DrawObject)_graphicsList[i]).GetXmlStr(scale);
            }
            return sXml;
        }

        public bool IsAnythingSelected()
        {
            foreach (DrawObject o in _graphicsList)
                if (o.Selected)
                    return true;

            return false;
        }

        public void Move(ArrayList movedItemsList, PointF delta)
        {
            var cmd = new MoveCommand(movedItemsList, delta);
            _undoRedo.AddCommand(cmd);
        }

        /// <summary>
        /// 将所选项移到后面（列表结尾）
        /// </summary>
        public bool MoveSelectionToBack()
        {
            int n = _graphicsList.Count;
            var tempList = new ArrayList();

            for (int i = n - 1; i >= 0; i--)
            {
                if (((DrawObject)_graphicsList[i]).Selected)
                {
                    tempList.Add(_graphicsList[i]);
                }
            }

            var cmd = new SendToBackCommand(_graphicsList, tempList);
            cmd.Execute();
            _undoRedo.AddCommand(cmd);

            return true;
        }

        /// <summary>
        /// 将所选项移到前面（列表的开头）
        /// </summary>
        public bool MoveSelectionToFront()
        {
            int n = _graphicsList.Count;
            var tempList = new ArrayList();

            for (int i = n - 1; i >= 0; i--)
            {
                if (((DrawObject)_graphicsList[i]).Selected)
                {
                    tempList.Add(_graphicsList[i]);
                }
            }

            var cmd = new BringToFrontCommand(_graphicsList, tempList);
            cmd.Execute();
            _undoRedo.AddCommand(cmd);

            return true;
        }

        /// <summary>
        /// 属性已更改
        /// </summary>
        public void PropertyChanged(GridItem itemChanged, object oldVal)
        {
            int i;
            int n = _graphicsList.Count;
            var tempList = new ArrayList();

            for (i = n - 1; i >= 0; i--)
            {
                if (((DrawObject)_graphicsList[i]).Selected)
                {
                    tempList.Add(_graphicsList[i]);
                }
            }

            var cmd = new PropertyChangeCommand(tempList, itemChanged, oldVal);

            _undoRedo.AddCommand(cmd);
        }

        public void Redo()
        {
            _undoRedo.Redo();
        }

        public void Resize(SizeF newscale, SizeF oldscale)
        {
            foreach (DrawObject o in _graphicsList)
                o.Resize(newscale, oldscale);
        }

        public void ResizeCommand(DrawObject obj, PointF old, PointF newP, int handle)
        {
            var cmd = new ResizeCommand(obj, old, newP, handle);
            _undoRedo.AddCommand(cmd);
        }

        public void RerotateCommand(DrawObject obj, PointF old, PointF newP)
        {
            var cmd = new ReRotateCommand(obj, old, newP);
            _undoRedo.AddCommand(cmd);
        }

        public void SelectAll()
        {
            foreach (DrawObject o in _graphicsList)
            {
                o.Selected = true;
            }
        }

        public void SelectInRectangle(RectangleF rectangle)
        {
            UnselectAll();

            foreach (DrawObject o in _graphicsList)
            {
                if (o.IntersectsWith(rectangle))
                    o.Selected = true;
            }
        }

        // *************************************************
        public void Undo()
        {
            _undoRedo.Undo();
        }

        public void UnselectAll()
        {
            foreach (DrawObject o in _graphicsList)
            {
                o.Selected = false;
            }
        }

        DrawObject CreateDrawObject(SVGUnit svge)
        {
            DrawObject o = null;
            switch (svge.getElementType())
            {
                case SVGUnit.SVGUnitType.typeLine:
                    o = DrawLineObject.Create((SVGLine)svge);
                    break;
                case SVGUnit.SVGUnitType.typeRect:
                    o = DrawRectangleObject.Create((SVGRect)svge);
                    break;
                case SVGUnit.SVGUnitType.typeEllipse:
                    o = DrawEllipseObject.Create((SVGEllipse)svge);
                    break;
                case SVGUnit.SVGUnitType.typePolyline:
                    o = DrawPolygonObject.Create((SVGPolyline)svge);
                    break;
                case SVGUnit.SVGUnitType.typeImage:
                    o = DrawImageObject.Create((SVGImage)svge);
                    break;
                case SVGUnit.SVGUnitType.typeText:
                    o = DrawTextObject.Create((SVGText)svge);
                    break;
                case SVGUnit.SVGUnitType.typeGroup:
                    o = CreateGroup((SVGGroup)svge);
                    break;
                case SVGUnit.SVGUnitType.typePath:
                    o = DrawPathObject.Create((SVGPath)svge);
                    break;
                case SVGUnit.SVGUnitType.typeDesc:
                    Description = ((SVGDesc)svge).Value;
                    break;
                default:
                    break;
            }
            return o;
        }

        DrawObject CreateGroup(SVGGroup svg)
        {
            DrawObject o = null;
            SVGUnit child = svg.getChild();
            if (child != null)
                AddFromSvg(child);
            return o;
        }

        #endregion Methods
    }
}
