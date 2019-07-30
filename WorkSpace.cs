﻿using DrawWork;
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

namespace HuaTuDemo
{
    public partial class WorkSpace : UserControl
    {
        #region Constructors

        public WorkSpace()
        {
            InitializeComponent();
            drawArea.ToolChanged += ResetToolSelection;
            drawArea.PageChanged += DrawAreaMousePan;
            drawArea.ItemsSelected += DrawAreaItemsSelected;
            drawArea.Initialize(this);
            
            ResizeDrawArea();
            drawArea.Refresh();
        }

        #endregion Constructors

        #region Delegates

        public delegate void OnGridOptionChanged(object sender, EventArgs e);

        public delegate void OnItemsInSelection(object sender, MouseEventArgs e);

        public delegate void OnMousePan(object sender, MouseEventArgs e);

        public delegate void OnScrollMade(object sender, ScrollEventArgs e);

        public delegate void OnToolDone(object sender, EventArgs e);

        public delegate void OnZoomDone(object sender, EventArgs e);

        #endregion Delegates

        #region Events

        public event OnGridOptionChanged GridChange;

        public event OnItemsInSelection ItemsSelected;

        public event OnMousePan MousePan;

        public event OnScrollMade ScrollMade;

        public event WorkArea.OnToolDone ToolDone;

        public event OnZoomDone ZoomDone;

        #endregion Events

        #region Methods

        public void BringShapelToFront()
        {
            drawArea.GraphicsList.MoveSelectionToFront();
            drawArea.Refresh();
        }

        public bool CheckDirty()
        {
            return drawArea.Dirty;
        }

        public void Copy()
        {
            drawArea.GraphicsList.CopySelection();
            drawArea.Refresh();
        }

        public void Cut()
        {
            drawArea.GraphicsList.CutSelection();
            drawArea.Refresh();
        }

        public void Delete()
        {
            drawArea.GraphicsList.DeleteSelection();
            drawArea.Refresh();
        }

        public DrawArea.DrawToolType GetCurrentTool()
        {
            return drawArea.ActiveTool;
        }

        public float GetCurrentZoom()
        {
            return drawArea.ScaleDraw.Width;
        }

        public bool GetGridOption()
        {
            return drawArea.DrawGrid;
        }

        public int GetMinorGrids()
        {
            return (int)drawArea.Xdivs;
        }

        public String GetSvgDescription()
        {
            return drawArea.Description;
        }

        public Size GetWorkAreaSize()
        {
            return new Size(drawArea.Size.Width - 1, drawArea.Size.Height - 1);
        }

        public void GridOptionChanged(object sender, EventArgs e)
        {
            if (GridChange != null)
                GridChange(sender, e);

            if (sender.GetType().ToString() == "System.Windows.Forms.CheckBox")
            {
                var option = (CheckBox)sender;
                drawArea.DrawGrid = option.Checked;
            }
            else if (sender.GetType().ToString() == "System.Windows.Forms.NumericUpDown")
            {
                drawArea.Xdivs = (float)(((NumericUpDown)sender).Value);
                drawArea.Ydivs = (float)(((NumericUpDown)sender).Value);
            }

            drawArea.Refresh();
        }

        public void OpenFile(String fileName)
        {
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(fileName);//从本地读取xml文件
                drawArea.LoadFromXml(reader);
                drawArea.FileName = fileName;
                ResizeDrawArea();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }

        public void Paste()
        {
            drawArea.GraphicsList.PasteSelection();
            drawArea.Refresh();
        }

        public void PropertyChanged(GridItem itemChanged, object oldVal)
        {
            drawArea.PropertyChanged(itemChanged, oldVal);
            drawArea.Refresh();
        }

        public void Redo()
        {
            drawArea.GraphicsList.Redo();
            drawArea.Refresh();
        }

        public void ResizeDrawArea()
        {
            drawArea.Top = ClientRectangle.Top - VerticalScroll.Value;
            drawArea.Left = ClientRectangle.Left - HorizontalScroll.Value;
            drawArea.Width = (int)drawArea.SizePicture.Width + 1;
            drawArea.Height = (int)drawArea.SizePicture.Height + 1;
        }

        public void RestoreScroll()
        {
            HorizontalScroll.Value = _scrollPersistance.X;
            VerticalScroll.Value = _scrollPersistance.Y;
        }

        public void SaveFile(String fileName)
        {
            System.IO.StreamWriter writer = null;

            if (String.IsNullOrEmpty(fileName))
            {
                //I've opened this file. So i know the file name
                if (!(String.IsNullOrEmpty(drawArea.FileName)))
                {
                    fileName = drawArea.FileName;
                }
                else
                {
                    var dlgSaveFileDialog = new SaveFileDialog();
                    dlgSaveFileDialog.Filter = @"SVG files (*.svg)|*.svg|All files (*.*)|*.*";

                    if (dlgSaveFileDialog.ShowDialog() == DialogResult.Cancel)
                    {
                        return;
                    }
                    fileName = dlgSaveFileDialog.FileName;
                }
            }

            try
            {
                writer = new System.IO.StreamWriter(fileName);
                drawArea.SaveToXml(writer);//保存DrawArea到XML
                MessageBox.Show(@"Save Done");
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (writer != null) writer.Close();
            }
        }

        public void SelectAll()
        {
            drawArea.GraphicsList.SelectAll();
            drawArea.Refresh();
        }

        public void SendShapeToBack()
        {
            drawArea.GraphicsList.MoveSelectionToBack();
            drawArea.Refresh();
        }

        public void SetDrawAreaProperties(Size size, String desc)
        {
            drawArea.Description = desc;
            drawArea.Size = size;
            drawArea.SizePicture = size;
            drawArea.Refresh();
        }

        public void SetGridDivs(float x, float y)
        {
            drawArea.Xdivs = x;
            drawArea.Ydivs = y;
            drawArea.Refresh();
        }

        public void SetTool(String tool)
        {
            switch (tool)
            {
                case "Pointer":
                    drawArea.ActiveTool = DrawArea.DrawToolType.Pointer;
                    break;

                case "Rectangle":
                    drawArea.ActiveTool = DrawArea.DrawToolType.Rectangle;
                    break;

                case "Ellipse":
                    drawArea.ActiveTool = DrawArea.DrawToolType.Ellipse;
                    break;

                case "Line":
                    drawArea.ActiveTool = DrawArea.DrawToolType.Line;
                    break;

                case "Pan":
                    drawArea.ActiveTool = DrawArea.DrawToolType.Pan;
                    break;

                case "Pencil":
                    drawArea.ActiveTool = DrawArea.DrawToolType.Polygon;
                    break;

                case "Text":
                    drawArea.ActiveTool = DrawArea.DrawToolType.Text;
                    break;

                case "Path":
                    drawArea.ActiveTool = DrawArea.DrawToolType.Path;
                    break;

                case "Image":
                    drawArea.ActiveTool = DrawArea.DrawToolType.Bitmap;
                    break;

                default:
                    drawArea.ActiveTool = DrawArea.DrawToolType.Pointer;
                    break;
            }
        }

        public void SetZoom(float zoom)
        {
            drawArea.DoScaling(new SizeF(zoom, zoom));
            ResizeDrawArea();
            drawArea.Refresh();
            ZoomDone((object)zoom, new EventArgs());
        }

        public void Undo()
        {
            drawArea.GraphicsList.Undo();
            drawArea.Refresh();
        }

        public void UnSelectAll()
        {
            drawArea.GraphicsList.UnselectAll();
            drawArea.Refresh();
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            _scrollPersistance.X = HorizontalScroll.Value;
            _scrollPersistance.Y = VerticalScroll.Value;

            if (ScrollMade != null)
                ScrollMade(this, se);
        }

        protected override Point ScrollToControl(Control activeControl)
        {
            return DisplayRectangle.Location;
        }

        void DrawAreaItemsSelected(object sender, MouseEventArgs e)
        {
            if (ItemsSelected != null)
                ItemsSelected(sender, e);
        }

        private void DrawAreaMousePan(object sender, MouseEventArgs e)
        {
            if (MousePan != null)
                MousePan(sender, e);
        }

        private void ResetToolSelection(object sender, EventArgs e)
        {
            if (ToolDone != null)
                ToolDone(sender, e);
        }
        /// <summary>
        /// 由main的timer调用 100ms一次
        /// </summary>
        public void Tick()
        {
            //刷新一次
            drawArea.Refresh();
        }
        #endregion Methods
    }
}
