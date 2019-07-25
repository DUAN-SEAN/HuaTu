using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HuaTu.Controls.Public.UnitPagePackage.UnitPage;

namespace HuaTuDemo
{
    public partial class WorkspaceHolder : UnitPage
    {
        #region Fields

        Point _start = new Point(-1, -1);
        int _xDiff, _yDiff, _intialScrollBarLocX, _intialScrollBarLocY;

        #endregion Fields

        #region Constructors

        public WorkspaceHolder()
        {
            InitializeComponent();
            svgDrawForm.ZoomDone += SvgDrawFormZoomDone;
            svgDrawForm.MousePan += SvgDrawFormMousePan;
            svgDrawForm.GridChange += SvgDrawFormGridChange;
            svgDrawForm.ScrollMade += SvgDrawFormScrollMade;
            rulerControl_left.HooverValue += RulerControlLeftHooverValue;
            rulerControl_top.HooverValue += RulerControlTopHooverValue;
        }

        #endregion Constructors

        #region Delegates

        public delegate void OnMouseMovementCaptured(object sender,RulerControl.HooverValueEventArgs e);

        #endregion Delegates    

        #region Methods

        private void AdjustRuler()
        {
            rulerControl_left.StartValue = svgDrawForm.VerticalScroll.Value / rulerControl_left.ZoomFactor;
            rulerControl_top.StartValue = svgDrawForm.HorizontalScroll.Value / rulerControl_left.ZoomFactor;

        }

        void RulerControlLeftHooverValue(object sender, RulerControl.HooverValueEventArgs e)
        {
            label_mouseX.Text = @"y" + (((int)e.Value));
        }

        void RulerControlTopHooverValue(object sender, RulerControl.HooverValueEventArgs e)
        {
            label_mouseY.Text = @"x" + (((int)e.Value));
        }

        void SvgDrawFormGridChange(object sender, EventArgs e)
        {
            svgDrawForm.SetGridDivs(rulerControl_top.Divisions,
                rulerControl_left.Divisions);

            if (sender != null)
            {
                if (sender.GetType().ToString() == "System.Windows.Forms.NumericUpDown")
                {
                    var ctl = (NumericUpDown)(sender);
                    rulerControl_left.Divisions = (int)ctl.Value;
                    rulerControl_top.Divisions = (int)ctl.Value;
                }
            }
        }

        void SvgDrawFormMousePan(object sender, MouseEventArgs e)
        {
            Point currentPosition = MousePosition;

            if (e.Button == MouseButtons.Left)
            {
                if ((_start.X == -1) && (_start.Y == -1))
                {
                    _start = currentPosition;
                    _intialScrollBarLocX = svgDrawForm.HorizontalScroll.Value;
                    _intialScrollBarLocY = svgDrawForm.VerticalScroll.Value;
                    System.Diagnostics.Debug.Print("Start :" + _start.X + " " + _start.Y);
                }
                else
                {
                    _xDiff = _start.X - currentPosition.X;
                    _yDiff = _start.Y - currentPosition.Y;

                    if (svgDrawForm.HorizontalScroll.Visible)
                    {
                        if (svgDrawForm.HorizontalScroll.Value + _xDiff + _intialScrollBarLocX >= 0)
                        {
                            svgDrawForm.HorizontalScroll.Value = _xDiff + _intialScrollBarLocX;
                        }
                        else
                        {
                            svgDrawForm.HorizontalScroll.Value = 0;
                        }
                    }

                    if (svgDrawForm.VerticalScroll.Visible)
                    {
                        if (svgDrawForm.VerticalScroll.Value + _yDiff + _intialScrollBarLocY >= 0)
                        {
                            svgDrawForm.VerticalScroll.Value = _yDiff + _intialScrollBarLocY;
                        }
                        else
                        {
                            svgDrawForm.VerticalScroll.Value = 0;
                        }
                    }
                }
            }
            else
            {
                _start.X = -1; _start.Y = -1;
            }

            AdjustRuler();
        }

        private void SvgDrawForm_Load(object sender, EventArgs e)
        {

        }

        void SvgDrawFormScrollMade(object sender, ScrollEventArgs e)
        {
            AdjustRuler();
        }

        void SvgDrawFormZoomDone(object sender, EventArgs e)
        {
            rulerControl_left.ZoomFactor = (float)sender;
            rulerControl_top.ZoomFactor = (float)sender;
            SvgDrawFormScrollMade(null, null);
        }

        private void SvgFormHolderResize(object sender, EventArgs e)
        {
            svgDrawForm.Top = rulerControl_top.Height;
            svgDrawForm.Left = rulerControl_left.Width;
            svgDrawForm.Width = ClientRectangle.Width - 40;
            svgDrawForm.Height = ClientRectangle.Height - 25;
            SvgDrawFormGridChange(null, null);
        }

        #endregion Methods
    }
}
