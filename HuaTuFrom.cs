using HuaTu.Controls.Public.Dockable.Base;
using HuaTu.Controls.Public.Dockable.Enum;
using HuaTuDemo.Tools.ToolForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static HuaTuDemo.DrawArea;

namespace HuaTuDemo
{
    public partial class HuaTuFrom : Form
    {
        #region 字段
        static int _counter;

        DockableFormInfo _infoDocumentProperties;
        DockableFormInfo _infoFilesMain;
        DockableFormInfo _infoShapeProperties;
        DockableFormInfo _infoToolbar;
        ShapeProperties _shapeProperties;
        WorkArea _svgMainFiles;
        WorkSpaceControlBox _svgProperties;
        ToolBox _toolBox;
        #endregion 字段
        public HuaTuFrom()
        {
            InitializeComponent();
            Intialize();
        }

        #region 函数
        private void Intialize()
        {
            _svgMainFiles = new WorkArea();
            _svgMainFiles.PageChanged += OnPageSelectionChanged;
            _svgMainFiles.ToolDone += OnToolDone;
            _svgMainFiles.ItemsSelected += SvgMainFilesItemsSelected;

            _toolBox = new ToolBox { Size = new Size(113, 165) };
            _toolBox.ToolSelectionChanged += ToolSelectionChanged;

            _svgProperties = new WorkSpaceControlBox();
            _svgProperties.ZoomChange += OnZoomChanged;
            _svgProperties.GridOptionChange += GridOptionChaged;
            _svgProperties.WorkAreaOptionChange += SvgPropertiesWorkAreaOptionChange;

            _infoFilesMain = _docker.Add(_svgMainFiles, DockAllowed.Fill, new Guid("a6402b80-2ebd-4fd3-8930-024a6201d001"));
            _infoFilesMain.ShowCloseButton = false;

            _infoToolbar = _docker.Add(_toolBox, DockAllowed.All, new Guid("a6402b80-2ebd-4fd3-8930-024a6201d002"));
            _infoToolbar.ShowCloseButton = false;

            _infoDocumentProperties = _docker.Add(_svgProperties, DockAllowed.All, new Guid("a6402b80-2ebd-4fd3-8930-024a6201d003"));
            _infoDocumentProperties.ShowCloseButton = false;

            _shapeProperties = new ShapeProperties();
            _shapeProperties.PropertyChanged += ShapePropertiesPropertyChanged;
            _infoShapeProperties = _docker.Add(_shapeProperties, DockAllowed.All, new Guid("a6402b80-2ebd-4fd3-8930-024a6201d004"));
            _infoShapeProperties.ShowCloseButton = false;

            //2019.7.30添加时间timer 用于tick
            timer.Interval = 35;
            timer.Tick += new EventHandler(Tick);
            timer.Enabled = true;
        }
        /// <summary>
        /// 该方法在主线程调用，100ms轮询一次
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        public void Tick(object o,EventArgs e)
        {
            _svgMainFiles.GetCurrentWorkForm().svgDrawForm.Tick();
        }
        public void OnZoomChanged(object sender, EventArgs e)
        {
            _svgMainFiles.OnZoomChange(sender, e);
        }

        private void AboutToolStripMenuItemClick(object sender, EventArgs e)
        {
            var objAbout = new AboutBox();
            objAbout.ShowDialog();
        }

        private void BringToFrontToolStripMenuItemClick(object sender, EventArgs e)
        {
            _svgMainFiles.GetCurrentWorkForm().svgDrawForm.BringShapelToFront();
        }

        private void CopyToolStripMenuItemClick(object sender, EventArgs e)
        {
            _svgMainFiles.GetCurrentWorkForm().svgDrawForm.Copy();
        }

        private void CutToolStripMenuItemClick(object sender, EventArgs e)
        {
            _svgMainFiles.GetCurrentWorkForm().svgDrawForm.Cut();
        }

        private void DeleteToolStripMenuItemClick(object sender, EventArgs e)
        {
            _svgMainFiles.GetCurrentWorkForm().svgDrawForm.Delete();
        }

        private void ExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (_svgMainFiles.CloseAll())
            {
                Close();
            }
        }

        private void GridOptionChaged(object sender, EventArgs e)
        {
            _svgMainFiles.GridOptionChanges(sender, e);
        }

       

        private void NewToolStripMenuItemNewClick(object sender, EventArgs e)
        {
            _svgMainFiles.AddNewPage("New:" + _counter++);


        }

        private void OnPageSelectionChanged(object sender, EventArgs e)
        {
            var tool = _svgMainFiles.GetCurrentTool();

            _svgProperties.SetZoom(_svgMainFiles.GetZoom());

            bool opt = _svgMainFiles.GetGridOption();
            int minorGrids = _svgMainFiles.GetMinorGrids();
            Size gridSize = _svgMainFiles.GetCurrentWorkForm().svgDrawForm.GetWorkAreaSize();
            String desc = _svgMainFiles.GetCurrentWorkForm().svgDrawForm.GetSvgDescription();

            _svgProperties.SetGridOption(opt, minorGrids, gridSize, desc);
            _toolBox.SetToolSelection(tool);
        }

        private void OnToolDone(object sender, EventArgs e)
        {
            //Set to pointer
            _toolBox.SetToolSelection(DrawToolType.Pointer);
            if (((DrawArea)sender).GraphicsList.SelectionCount == 0)
            {
                _shapeProperties.propertyGrid.SelectedObject = null;
            }
        }

        private void OpenToolStripMenuItemClick(object sender, EventArgs e)
        {
            var flgOpenFileDialog = new OpenFileDialog();
            flgOpenFileDialog.Filter = @"SVG files (*.svg)|*.svg|All files (*.*)|*.*";

            if (flgOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                _svgMainFiles.OpenDocument(flgOpenFileDialog.FileName);
            }
        }

        private void PasteToolStripMenuItemClick(object sender, EventArgs e)
        {
            _svgMainFiles.GetCurrentWorkForm().svgDrawForm.Paste();
        }

        private void RedoToolStripMenuItemClick(object sender, EventArgs e)
        {
            _svgMainFiles.GetCurrentWorkForm().svgDrawForm.Redo();
        }

        private void SaveToolStripMenuItemClick(object sender, EventArgs e)
        {
            var dlgSaveFileDialog = new SaveFileDialog();
            dlgSaveFileDialog.Filter = @"SVG files (*.svg)|*.svg|All files (*.*)|*.*";
            if (dlgSaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                _svgMainFiles.SaveDocument(dlgSaveFileDialog.FileName);
            }
        }

        private void SendBackToolStripMenuItemClick(object sender, EventArgs e)
        {
            _svgMainFiles.GetCurrentWorkForm().svgDrawForm.SendShapeToBack();
        }

        void ShapePropertiesPropertyChanged(object sender, PropertyValueChangedEventArgs e)
        {
            _svgMainFiles.PropertyChanged(e.ChangedItem, e.OldValue);
        }

        void SvgMainFilesItemsSelected(object sender, MouseEventArgs e)
        {
            int i = 0;
            var selectedItems = (List<DrawWork.DrawObject>)sender;
            var obj = new object[selectedItems.Count];
            foreach (DrawWork.DrawObject dob in selectedItems)
            {
                obj[i++] = dob;
            }

            if (selectedItems.Count > 0)
            {
                _shapeProperties.propertyGrid.SelectedObjects = obj;
            }
        }

        private void SvgMainShown(object sender, EventArgs e)
        {
            _docker.DockForm(_infoToolbar, DockStyle.Left, DockableMode.Inner);
            _docker.DockForm(_infoFilesMain, DockStyle.Fill, DockableMode.Inner);
            _docker.DockForm(_infoDocumentProperties, DockStyle.Right, DockableMode.Inner);
            _docker.DockForm(_infoShapeProperties, _infoToolbar, DockStyle.Bottom, DockableMode.Outer);
            _svgMainFiles.AddNewPage("New:" + _counter++);

        }

        void SvgPropertiesWorkAreaOptionChange(object sender, WorkSpaceControlBox.ControlBoxEventArgs e)
        {
            _svgMainFiles.GetCurrentWorkForm().svgDrawForm.SetDrawAreaProperties(e.Size, e.Description);
        }

        private void ToolSelectionChanged(object sender, EventArgs e)
        {
            _svgMainFiles.SetTool((String)sender);
        }

        private void ToolStripButtonCloseClick(object sender, EventArgs e)
        {
            _svgMainFiles.CloseActiveDocument();
        }

        private void TsSelectAllClick(object sender, EventArgs e)
        {
            _svgMainFiles.GetCurrentWorkForm().svgDrawForm.SelectAll();
        }

        private void UndoToolStripMenuItemClick(object sender, EventArgs e)
        {
            _svgMainFiles.GetCurrentWorkForm().svgDrawForm.Undo();
        }

        private void UnSelectAllToolStripMenuItemClick(object sender, EventArgs e)
        {
            _svgMainFiles.GetCurrentWorkForm().svgDrawForm.UnSelectAll();
        }

        #endregion 函数

        private void HuaTuFrom_Load(object sender, EventArgs e)
        {

        }

        private void FileToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

    }
}
