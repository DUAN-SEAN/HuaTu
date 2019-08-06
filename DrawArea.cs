using DrawWork;
using HuaTuDemo.Tools;
using SVGHelper;
using SVGHelper.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using DrawWork.Symbol;

namespace HuaTuDemo
{
    public partial class DrawArea : UserControl
    {
        #region 字段

        public ArrayList ChartValues = new ArrayList();
        public Boolean Dirty;
        public float ScaleX, ScaleY;
        public string Title = "Default Title";
        public float Xdivs = 2, Ydivs = 2, MajorIntervals = 100;
        public float Xorigin, Yorigin;

        private IContainer components;

        // (instances of DrawObject-derived classes)
        private DrawToolType _activeTool; // active drawing tool
        private ToolStripMenuItem _bringToFrontToolStripMenuItem;
        private ContextMenuStrip _contextMenuStrip;
        private ToolStripMenuItem _copyToolStripMenuItem;
        private ToolStripMenuItem _cutToolStripMenuItem;
        private ToolStripMenuItem _deleteToolStripMenuItem;
        private DrawObjectList _graphicsList; // list of draw objects
        private string _mDescription = "Svg picture";
        private SizeF _mOriginalSize = new SizeF(500, 400);

        // group selection rectangle
        // Information about owner form
        private SizeF _mScale = new SizeF(1.0f, 1.0f);
        private SizeF _mSizePicture = new SizeF(500, 400);
        private ToolStripMenuItem _pasteToolStripMenuItem;
        private ToolStripMenuItem _selectAllToolStripMenuItem;
        private ToolStripMenuItem _sendToBackToolStripMenuItem;
        private Tool[] _tools; // array of tools
        private ToolStripSeparator _toolStripSeparator1;
        private ToolStripSeparator _toolStripSeparator2;
        private ToolStripSeparator _toolStripSeparator3;
        private int _width, _height;

        public String FileName { get; set; }

        #endregion 字段

        #region 构造器

        public DrawArea()
        {
            _height = 500;
            _width = 400;

            InitializeComponent();
        }

        #endregion 构造器

        #region 枚举

        public enum DrawToolType
        {
            Pointer,
            Rectangle,
            Ellipse,
            Line,
            Polygon,
            Bitmap,
            Text,
            Pan,
            Path,
            NumberOfDrawTools
        }

        #endregion 枚举

        #region 委托定义
        /// <summary>
        /// 在对象被选择时调用
        /// </summary>
        public delegate void OnItemSelected(object sender, MouseEventArgs e);

        public delegate void OnMousePan(object sender, MouseEventArgs e);

        public delegate void OnMouseSelectionDone(object sender, EventArgs e);
        #endregion

        #region 注册事件
        public event OnItemSelected ItemsSelected;

        public event OnMousePan PageChanged;

        public event OnMouseSelectionDone ToolChanged;
        #endregion

        #region 属性

        /// <summary>
        /// 激活的绘图工具
        /// </summary>
        public DrawToolType ActiveTool
        {
            get
            {
                return _activeTool;
            }
            set
            {
                if (_tools != null)
                    _tools[(int)_activeTool].ToolActionCompleted();
                _activeTool = value;
            }
        }

        public string Description
        {
            get
            {
                return _mDescription;
            }
            set
            {
                _mDescription = value;
            }
        }

        /// <summary>
        /// 决定是否显示网格
        /// </summary>
        public bool DrawGrid
        {
            get; set;
        }

        /// <summary>
        /// 如果应绘制组选择矩形，则flas设置为true。
        /// </summary>
        public bool DrawNetRectangle
        {
            get; set;
        }

        /// <summary>
        /// 图形对象列表。
        /// </summary>
        [CLSCompliant(false)]
        public DrawObjectList GraphicsList
        {
            get
            {
                return _graphicsList;
            }
            set
            {
                _graphicsList = value;
            }
        }

        /// <summary>
        /// 分组选择矩形。用于绘图。
        /// </summary>
        public RectangleF NetRectangle
        {
            get; set;
        }

        public SizeF OldScale
        {
            get
            {
                return _mScale;
            }
            set
            {
                _mScale = value;
            }
        }

        public SizeF OriginalSize
        {
            get
            {
                return _mOriginalSize;
            }
            set
            {
                _mOriginalSize = value;
            }
        }

        /// <summary>
        /// 对拥有者的引用
        /// </summary>
        public Control Owner
        {
            get; set;
        }

        public SizeF ScaleDraw
        {
            get
            {
                return _mScale;
            }
            set
            {
                _mScale = value;
            }
        }

        public SizeF SizePicture
        {
            get
            {
                return _mSizePicture;
            }
            set
            {
                _mSizePicture = value;
            }
        }

        #endregion 属性

        #region Methods

        public void DoScaling(SizeF sc)
        {
            DrawObject.Zoom = sc.Height;
            _graphicsList.Resize(sc, _mScale);
            _mScale = sc;
            _mSizePicture = new SizeF(_mScale.Width * OriginalSize.Width,
                _mScale.Height * OriginalSize.Height);
        }

        public void Draw(Graphics g)
        {
            var brush = new SolidBrush(Color.FromArgb(255, 255, 255));
            g.FillRectangle(brush, ClientRectangle);
            // draw rect svg size
            var pen = new Pen(Color.FromArgb(0, 0, 255), 1);
            g.DrawRectangle(pen, 0, 0, SizePicture.Width, SizePicture.Height);
            if (_graphicsList != null)
            {
                _graphicsList.Draw(g);
            }
            brush.Dispose();
        }

        /// <summary>
        ///  绘制组选择矩形
        /// </summary>
        public void DrawNetSelection(Graphics g)
        {
            if (!DrawNetRectangle)
                return;
            var r = new Rectangle(Convert.ToInt32(NetRectangle.X), Convert.ToInt32(NetRectangle.Y),
                Convert.ToInt32(NetRectangle.Width), Convert.ToInt32(NetRectangle.Height));
            ControlPaint.DrawFocusRectangle(g, r, Color.Black, Color.Transparent);
        }

        /// <summary>
        /// 实例化
        /// </summary>
        public void Initialize(Control owner)
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);

            // Keep reference to owner form
            Owner = owner;

            // set default tool
            _activeTool = DrawToolType.Pointer;

            // create list of graphic objects
            _graphicsList = new DrawObjectList();

            // create array of drawing tools
            _tools = new Tool[(int)DrawToolType.NumberOfDrawTools];
            _tools[(int)DrawToolType.Pointer] = new ToolPointer();
            _tools[(int)DrawToolType.Rectangle] = new ToolRectangle();
            _tools[(int)DrawToolType.Ellipse] = new ToolEllipse();
            _tools[(int)DrawToolType.Line] = new ToolLine();
            _tools[(int)DrawToolType.Polygon] = new ToolPolygon();
            _tools[(int)DrawToolType.Text] = new ToolText();
            _tools[(int)DrawToolType.Bitmap] = new ToolImage();
            _tools[(int)DrawToolType.Pan] = new ToolPan();
            _tools[(int)DrawToolType.Path] = new ToolPath();

            Graphics g = Owner.CreateGraphics();
            DrawObject.Dpi = new PointF(g.DpiX, g.DpiY);
        }

        public bool LoadFromXml(XmlTextReader reader)
        {
            SVGErr.Log("DrawArea", "LoadFromXML", "", SVGErr._LogPriority.Info);
            _graphicsList.Clear();
            var svg = new SVGWord();
            if (!svg.LoadFromFile(reader))
                return false;
            SVGRoot root = svg.GetSvgRoot();

            if (root == null)
                return false;
            try
            {
                SizePicture = new SizeF(DrawObject.ParseSize(root.Width, DrawObject.Dpi.X),
                    DrawObject.ParseSize(root.Height, DrawObject.Dpi.Y));
            }
            catch
            {
            }
            _mOriginalSize = SizePicture;
            SVGUnit ele = root.getChild();
            _mScale = new SizeF(1, 1);
            if (ele != null)
            {

                //1 收集symbol获取svg上的设备
                //2 从svg元数据中收集symbol之间的关系
                //3 将所有use的设备实体生成
                //4 绘制list集合将图素绘制出来
                SVGFactory.CreateProjectFromXML(ele);


                //_graphicsList.AddFromSvg(ele);
            }


            Description = _graphicsList.Description;
            return true;
        }

        public void MkResize()
        {
            SizeF oldscale = _mScale;
            _mScale.Width = _width / _mOriginalSize.Width;
            _mScale.Height = _height / _mOriginalSize.Height;
            _graphicsList.Resize(_mScale, oldscale);
            SizePicture = new SizeF(DrawObject.RecalcFloat(SizePicture.Width, _mScale.Width, oldscale.Width),
                DrawObject.RecalcFloat(SizePicture.Height, _mScale.Height, oldscale.Height));
        }

        public void RestoreScale()
        {
            _graphicsList.Resize(new SizeF(1, 1), _mScale);
            _mScale = new SizeF(1, 1);
        }
        /// <summary>
        /// 保存到svg中
        /// </summary>
        /// <param name="sw"></param>
        /// <returns></returns>
        public bool SaveToXml(StreamWriter sw)
        {
            try
            {
                const string mSXmlDeclaration = "<?xml version=\"1.0\" standalone=\"no\"?>";
                const string mSXmlDocType = "<!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.1//EN\" \"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd \">";

                string sXml = mSXmlDeclaration + "\r\n";
                sXml += mSXmlDocType + "\r\n";
                sXml += "<svg xmlns=\"http://www.w3.org/2000/svg\"  xmlns:xlink=\"http://www.w3.org/1999/xlink\"  xmlns:cge=\"http://www.cim.com\" version=\"1.1\" width=\"" + _mOriginalSize.Width.ToString(CultureInfo.InvariantCulture) +
                "\" height=\"" + _mOriginalSize.Height.ToString(CultureInfo.InvariantCulture) + "\">" + "\r\n";
                sXml += "<desc>" + Description + "</desc>" + "\r\n";
                //sXml += _graphicsList.GetXmlString(_mScale);//将每一个图形都转换为标准格式
                sXml += SVGFactory.GenerateSVGXml(_mScale,null);//改为新的svg内容
                sXml += "</svg>" + "\r\n";
                sw.Write(sXml);
                sw.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 设置脏标志（文件在上次保存操作后更改）
        /// </summary>
        public void SetDirty()
        {
            Dirty = true;
        }

        /// <summary> 
        /// 清除所有正在使用的资源。
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// 鼠标向下。
        /// 左键向下事件传递给活动工具。
        /// 在此类中处理右键向下事件。
        /// </summary>
        private void DrawArea_MouseDown(object sender, MouseEventArgs e)
        {
            if (!_tools[(int)_activeTool].IsComplete)
            {
                _tools[(int)_activeTool].OnMouseDown(this, e);
                if (e.Button == MouseButtons.Right)
                {
                    if (_tools[(int)_activeTool].IsComplete)
                    {
                        _activeTool = DrawToolType.Pointer;
                        ToolChanged(sender, e);
                        Refresh();
                    }
                }
                return;
            }
            if (e.Button == MouseButtons.Left)
                _tools[(int)_activeTool].OnMouseDown(this, e);
            else if (e.Button == MouseButtons.Right)
                OnContextMenu(e);

            if (_graphicsList.IsAnythingSelected() && (!_tools[(int)_activeTool].IsComplete))
            {
                if (ItemsSelected != null)
                    ItemsSelected(_graphicsList.GetAllSelected(), e);
            }
        }

        /// <summary>
        /// 鼠标移动。
        /// 不按按钮或按左键移动
        /// 传递给活动工具。
        /// </summary>
        private void DrawArea_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left || e.Button == MouseButtons.None)
                {
                    if (_activeTool == DrawToolType.Pan)
                    {
                        if (PageChanged != null)
                        {
                            PageChanged(sender, e);
                        }
                    }

                    var ind = (int)_activeTool;
                    _tools[ind].OnMouseMove(this, e);
                }
                else
                    Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                SVGErr.Log("DrawArea", "DrawArea_MouseMove", ex.ToString(), SVGErr._LogPriority.Info);
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 鼠标悬停事件。
        /// 左键向上事件传递给活动工具。
        /// </summary>
        private void DrawArea_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _tools[(int)_activeTool].OnMouseUp(this, e);
                bool res = _tools[(int)_activeTool].IsComplete;

                // if (activeTool != DrawToolType.Pan)
                if (res)
                {
                    ToolChanged(sender, e);
                    ActiveTool = DrawToolType.Pointer;
                }
                else
                {
                    Refresh();
                }
            }

            if (_graphicsList.GetAllSelected().Count > 0)
            {
                if (ItemsSelected != null)
                    ItemsSelected(_graphicsList.GetAllSelected(), e);
            }
        }

        /// <summary>
        /// 绘制图形对象和
        /// 分组选择矩形（可选）
        /// </summary>
        private void DrawArea_Paint(object sender, PaintEventArgs e)
        {
            var brush = new SolidBrush(Color.FromArgb(255, 255, 255));
            e.Graphics.FillRectangle(brush,
                ClientRectangle);
            // draw rect svg size

            if (DrawGrid)
                DrawGridsAndScale(e.Graphics);

            if (_graphicsList != null)
                _graphicsList.Draw(e.Graphics);

            DrawNetSelection(e.Graphics);
            brush.Dispose();
        }

        private void DrawGridsAndScale(Graphics g)
        {
            var majorlinesPen = new Pen(Color.Wheat, 1);
            var minorlinesPen = new Pen(Color.LightGray, 1);

            Xorigin = Yorigin = 0;
            ScaleX = _width;
            ScaleY = _height;

            _width = (int)(SizePicture.Width);
            _height = (int)(SizePicture.Height);

            var xMajorLines = (int)(_width / MajorIntervals / ScaleDraw.Width);
            var yMajorLines = (int)(_height / MajorIntervals / ScaleDraw.Height);

            try
            {
                //draw X Axis major lines
                for (int i = 0; i <= xMajorLines; i++)
                {
                    float x = i * (_width / xMajorLines);
                    g.DrawLine(minorlinesPen, x, 0, x, _height);

                    //draw X Axis minor lines
                    for (int i1 = 1; i1 <= Xdivs; i1++)
                    {
                        float x1 = i1 * MajorIntervals / (Xdivs) * ScaleDraw.Width;
                        g.DrawLine(majorlinesPen, x + x1, 0, x + x1, _height);
                    }
                }

                //draw Y Axis major lines
                for (int i = 0; i <= yMajorLines; i++)
                {
                    //y = i * (Height / (yMajorLines));
                    //float y = i * MajorIntervals * ScaleDraw.Height;
                    float y = i * _height / yMajorLines;

                    g.DrawLine(minorlinesPen, 0, y, _width, y);

                    //draw Y Axis minor lines
                    for (int i1 = 1; i1 <= Ydivs; i1++)
                    {
                        float y1 = i1 * MajorIntervals / (Ydivs) * ScaleDraw.Height;
                        g.DrawLine(majorlinesPen, 0, y + y1, _width, y + y1);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary> 
        /// 设计器支持所需的方法-不要修改
        /// 此方法的内容和代码编辑器。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._bringToFrontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._sendToBackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this._deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this._cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _contextMenuStrip
            // 
            this._contextMenuStrip.ImageScalingSize = new System.Drawing.Size(28, 28);
            this._contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._selectAllToolStripMenuItem,
            this._toolStripSeparator1,
            this._bringToFrontToolStripMenuItem,
            this._sendToBackToolStripMenuItem,
            this._toolStripSeparator3,
            this._deleteToolStripMenuItem,
            this._toolStripSeparator2,
            this._cutToolStripMenuItem,
            this._copyToolStripMenuItem,
            this._pasteToolStripMenuItem});
            this._contextMenuStrip.Name = "_contextMenuStrip";
            this._contextMenuStrip.Size = new System.Drawing.Size(224, 260);
            // 
            // _selectAllToolStripMenuItem
            // 
            this._selectAllToolStripMenuItem.Name = "_selectAllToolStripMenuItem";
            this._selectAllToolStripMenuItem.Size = new System.Drawing.Size(223, 34);
            this._selectAllToolStripMenuItem.Text = "Select All";
            // 
            // _toolStripSeparator1
            // 
            this._toolStripSeparator1.Name = "_toolStripSeparator1";
            this._toolStripSeparator1.Size = new System.Drawing.Size(220, 6);
            // 
            // _bringToFrontToolStripMenuItem
            // 
            this._bringToFrontToolStripMenuItem.Name = "_bringToFrontToolStripMenuItem";
            this._bringToFrontToolStripMenuItem.Size = new System.Drawing.Size(223, 34);
            this._bringToFrontToolStripMenuItem.Text = "Bring to Front";
            // 
            // _sendToBackToolStripMenuItem
            // 
            this._sendToBackToolStripMenuItem.Name = "_sendToBackToolStripMenuItem";
            this._sendToBackToolStripMenuItem.Size = new System.Drawing.Size(223, 34);
            this._sendToBackToolStripMenuItem.Text = "Send to Back";
            // 
            // _toolStripSeparator3
            // 
            this._toolStripSeparator3.Name = "_toolStripSeparator3";
            this._toolStripSeparator3.Size = new System.Drawing.Size(220, 6);
            // 
            // _deleteToolStripMenuItem
            // 
            this._deleteToolStripMenuItem.Name = "_deleteToolStripMenuItem";
            this._deleteToolStripMenuItem.Size = new System.Drawing.Size(223, 34);
            this._deleteToolStripMenuItem.Text = "Delete";
            // 
            // _toolStripSeparator2
            // 
            this._toolStripSeparator2.Name = "_toolStripSeparator2";
            this._toolStripSeparator2.Size = new System.Drawing.Size(220, 6);
            // 
            // _cutToolStripMenuItem
            // 
            this._cutToolStripMenuItem.Name = "_cutToolStripMenuItem";
            this._cutToolStripMenuItem.Size = new System.Drawing.Size(223, 34);
            this._cutToolStripMenuItem.Text = "Cut";
            // 
            // _copyToolStripMenuItem
            // 
            this._copyToolStripMenuItem.Name = "_copyToolStripMenuItem";
            this._copyToolStripMenuItem.Size = new System.Drawing.Size(223, 34);
            this._copyToolStripMenuItem.Text = "Copy";
            // 
            // _pasteToolStripMenuItem
            // 
            this._pasteToolStripMenuItem.Name = "_pasteToolStripMenuItem";
            this._pasteToolStripMenuItem.Size = new System.Drawing.Size(223, 34);
            this._pasteToolStripMenuItem.Text = "Paste";
            // 
            // DrawArea
            // 
            this.AutoScroll = true;
            this.AutoSize = true;
            this.Name = "DrawArea";
            this.Size = new System.Drawing.Size(153, 136);
            this.Paint += DrawArea_Paint;
            this.MouseMove += DrawArea_MouseMove;
            this.MouseDown += DrawArea_MouseDown;
            this.MouseUp += DrawArea_MouseUp;
            this._contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            //this.PerformLayout();

        }

        /// <summary>
        /// 右键单击处理程序
        /// </summary>
        private void OnContextMenu(MouseEventArgs e)
        {
            // Change current selection if necessary

            var point = new Point(e.X, e.Y);

            int n = GraphicsList.Count;
            DrawObject o = null;

            for (int i = 0; i < n; i++)
            {
                if (GraphicsList[i].HitTest(point) == 0)
                {
                    o = GraphicsList[i];
                    break;
                }
            }

            if (o != null)
            {
                if (!o.Selected)
                    GraphicsList.UnselectAll();
                // Select clicked object
                o.Selected = true;
                _bringToFrontToolStripMenuItem.Enabled = true;
                _sendToBackToolStripMenuItem.Enabled = true;
                _cutToolStripMenuItem.Enabled = true;
                _copyToolStripMenuItem.Enabled = true;
                _deleteToolStripMenuItem.Enabled = true;
            }
            else
            {
                _bringToFrontToolStripMenuItem.Enabled = false;
                _sendToBackToolStripMenuItem.Enabled = false;
                _cutToolStripMenuItem.Enabled = false;
                _copyToolStripMenuItem.Enabled = false;
                _deleteToolStripMenuItem.Enabled = false;
                GraphicsList.UnselectAll();
            }

            _pasteToolStripMenuItem.Enabled = GraphicsList.AreItemsInMemory();
            _contextMenuStrip.Show(MousePosition);
            Refresh();
        }

        private void CutToolStripMenuItemClick(object sender, EventArgs e)
        {
            _graphicsList.CutSelection();
            Refresh();
        }

        private void CopyToolStripMenuItemClick(object sender, EventArgs e)
        {
            _graphicsList.CopySelection();
            Refresh();
        }

        private void PasteToolStripMenuItemClick(object sender, EventArgs e)
        {
            _graphicsList.PasteSelection();
            Refresh();
        }

        private void SelectAllToolStripMenuItemClick(object sender, EventArgs e)
        {
            _graphicsList.SelectAll();
            Refresh();
        }

        private void SendToBackToolStripMenuItemClick(object sender, EventArgs e)
        {
            _graphicsList.MoveSelectionToBack();
            Refresh();
        }

        

        private void BringToFrontToolStripMenuItemClick(object sender, EventArgs e)
        {
            _graphicsList.MoveSelectionToFront();
            Refresh();
        }

        private void DeleteToolStripMenuItemClick(object sender, EventArgs e)
        {
            _graphicsList.DeleteSelection();
            Refresh();
        }

        public void MoveCommand(ArrayList movedItemsList, PointF delta)
        {
            _graphicsList.Move(movedItemsList, delta);
            Refresh();
        }

        public void PropertyChanged(GridItem itemChanged, object oldVal)
        {
            _graphicsList.PropertyChanged(itemChanged, oldVal);
        }

        public void ResizeCommand(DrawObject resizedItems, PointF old, PointF newP, int handle)
        {
            _graphicsList.ResizeCommand(resizedItems, old, newP, handle);
            Refresh();
        }

        public void RerotateCommand(DrawObject rerotateItems, PointF old, PointF newP)
        {
            _graphicsList.RerotateCommand(rerotateItems, old, newP);
            Refresh();
        }
        #endregion Methods


    }
}
