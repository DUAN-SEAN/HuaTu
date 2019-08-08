using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using DrawWork;
using DrawWork.Symbol;
using SVGHelper;
using SVGHelper.Base;
using static HuaTuDemo.DrawArea;

namespace HuaTuDemo
{
    public partial class WorkArea : Form
    {
        #region Constructors

        public WorkArea()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Delegates

        public delegate void OnItemsInSelection(object sender, MouseEventArgs e);

        public delegate void OnTabPageChanged(object sender, EventArgs e);

        public delegate void OnToolDone(object sender, EventArgs e);

        #endregion Delegates

        #region Events

        public event OnItemsInSelection ItemsSelected;

        public event OnTabPageChanged PageChanged;

        public event OnToolDone ToolDone;

        #endregion Events

        #region Methods

        public void AddNewPage(String fileName)
        {

            var svgForm = new WorkspaceHolder { Dock = DockStyle.Fill, Name = fileName };
            svgForm.svgDrawForm.ToolDone += OnToolDoneComplete;
            svgForm.svgDrawForm.ItemsSelected += SvgDrawFormItemsSelected;
            tabbedView.Add(svgForm);
        }

        public void CloseActiveDocument()
        {
            if (tabbedView.Count > 0)
            {
                var svgForm = (WorkspaceHolder)tabbedView.GetPageAt(tabbedView.SelectedIndex);
                if (!svgForm.svgDrawForm.CheckDirty())
                {
                    tabbedView.Remove(svgForm);
                }
                else
                {
                    if ((MessageBox.Show(@"Changes have been made. Exit without Saving?", @"SVG Editor", MessageBoxButtons.YesNo) == DialogResult.Yes))
                    {
                        tabbedView.Remove(svgForm);
                    }
                }
            }
        }

        public bool CloseAll()
        {
            bool retVal = true;
            int count = tabbedView.Count;
            if (tabbedView.Count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    var svgForm = (WorkspaceHolder)tabbedView.GetPageAt(0);
                    if (!svgForm.svgDrawForm.CheckDirty())
                    {
                        tabbedView.Remove(svgForm);
                    }
                    else
                    {
                        if ((MessageBox.Show(@"Changes have been made. Exit without Saving?", @"SVG Editor", MessageBoxButtons.YesNo) == DialogResult.Yes))
                        {
                            tabbedView.SelectedIndex = 0;
                            tabbedView.Remove(svgForm);
                        }
                        else
                        {
                            tabbedView.SelectedIndex = 0;
                            retVal = false;
                        }
                    }
                }
            }
            return retVal;
        }

        public DrawToolType GetCurrentTool()
        {
            var svgForm = (WorkspaceHolder)tabbedView.GetPageAt(tabbedView.SelectedIndex);
            return svgForm.svgDrawForm.GetCurrentTool();
        }

        public WorkspaceHolder GetCurrentWorkForm()
        {
            return (WorkspaceHolder)tabbedView.GetPageAt(tabbedView.SelectedIndex);
        }

        public bool GetGridOption()
        {
            var svgForm = (WorkspaceHolder)tabbedView.GetPageAt(tabbedView.SelectedIndex);
            return svgForm.svgDrawForm.GetGridOption();
        }

        public int GetMinorGrids()
        {
            return GetCurrentForm().GetMinorGrids();
        }

        public WorkSpace GetCurrentForm()
        {
            return ((WorkspaceHolder)tabbedView.GetPageAt(tabbedView.SelectedIndex)).svgDrawForm;
        }

        public int GetWorkAreaSize()
        {
            return GetCurrentForm().GetMinorGrids();
        }

        public int GetZoom()
        {
            return (int)(GetCurrentForm().GetCurrentZoom());
        }

        public void GridOptionChanges(object sender, EventArgs e)
        {
            GetCurrentForm().GridOptionChanged(sender, e);
        }

        public void OnZoomChange(object sender, EventArgs e)
        {
            var tb = (TrackBar)sender;
            if (tb != null)
            {
                GetCurrentForm().SetZoom(tb.Value);
            }
        }

        public void OpenDocument(String fileName)
        {
            var svgForm = new WorkspaceHolder { Dock = DockStyle.Fill, Name = fileName };
            svgForm.svgDrawForm.ToolDone += OnToolDoneComplete;
            svgForm.svgDrawForm.ItemsSelected += SvgDrawFormItemsSelected;
            svgForm.svgDrawForm.OpenFile(fileName);
            tabbedView.Add(svgForm);
            svgForm.Refresh();
        }

        public bool LoadSvgModel(String fileName)
        {
           return LoadModelFromXml(fileName);
        }

        public bool LoadModelFromXml(string fileName)
        {
            var svgForm = new WorkspaceHolder { Dock = DockStyle.Fill, Name = fileName };
            svgForm.svgDrawForm.ToolDone += OnToolDoneComplete;
            svgForm.svgDrawForm.ItemsSelected += SvgDrawFormItemsSelected;

            svgForm.svgDrawForm.drawArea.Width = 1600;
            svgForm.svgDrawForm.drawArea.Height = 900;

            XmlTextReader reader = null;
            //XmlReader reader = null;
            //var txt = File.ReadAllText(fileName);
            try
            {
               // FileStream fs = new FileStream(fileName,FileMode.Open);
                

                //reader =  XmlReader.Create(fileName);
                reader = new XmlTextReader(fileName);//从本地读取xml文件

                SVGErr.Log("DrawArea", "LoadFromXML", "", SVGErr._LogPriority.Info);
                var svg = new SVGWord();
                if (!svg.LoadFromFile(reader))
                    return false;
                SVGRoot root = svg.GetSvgRoot();

                if (root == null)
                    return false;
                SVGUnit ele = root.getChild();
                if (ele != null)
                {

                    //1 收集symbol获取svg上的设备
                    //2 从svg元数据中收集symbol之间的关系
                    //3 将所有use的设备实体生成
                    //4 绘制list集合将图素绘制出来
                    SVGFactory.CreateProjectFromXML(ele, svgForm.svgDrawForm.drawArea.GraphicsList);
                }

            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            
            tabbedView.Add(svgForm);
            svgForm.Refresh();

            return true;
        }
        public void PropertyChanged(GridItem itemChanged, object oldVal)
        {
            GetCurrentForm().PropertyChanged(itemChanged, oldVal);
        }

        public void SaveDocument(String fileName)
        {
            GetCurrentForm().SaveFile(fileName);
        }

        public void SetTool(String tool,EventArgs e = null)
        {
            GetCurrentForm().SetTool(tool,e);
        }

        private void OnToolDoneComplete(object sender, EventArgs e)
        {
            ToolDone(sender, e);
        }

        void SvgDrawFormItemsSelected(object sender, MouseEventArgs e)
        {
            if (ItemsSelected != null)
                ItemsSelected(sender, e);
        }

        private void TabbedViewPageSelectionMade(object sender, EventArgs e)
        {
            PageChanged(sender, e);
        }

        #endregion Methods

        private void TabbedView_Load(object sender, EventArgs e)
        {

        }
    }
}
