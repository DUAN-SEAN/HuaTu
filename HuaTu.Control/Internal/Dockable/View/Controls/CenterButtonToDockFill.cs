using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace HuaTu.Controls.Internal.Dockable.View.Controls
{
    /// <summary>
    /// Center button for guiding dock fill
    /// </summary>
    internal partial class CenterButtonToDockFill : Control
    {
        #region Fields

        private bool _showFillPreview = false;

        #endregion Fields

        #region Instance

        /// <summary>
        /// Default constructor
        /// </summary>
        public CenterButtonToDockFill()
        {
            InitializeComponent();

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddLine(0, 6, 6, 0);
                path.AddLine(35, 0, 41, 6);
                path.AddLine(41, 35, 35, 41);
                path.AddLine(6, 41, 0, 35);

                Region = new Region(path);
            }

            _fillImage.Left = (Width - _fillImage.Width) / 2;
            _fillImage.Top = (Height - _fillImage.Height) / 2;
        }

        #endregion Instance

        #region Public section

        /// <summary>
        /// Show fill preview button inside the form
        /// </summary>
        public bool ShowFillPreview
        {
            get { return _showFillPreview; }
            set
            {
                _showFillPreview = value;
                _fillImage.Visible = value;
            }
        }

        /// <summary>
        /// Fill size
        /// </summary>
        public Size FillSize
        {
            get
            {
                return _fillImage.Size;
            }
        }

        /// <summary>
        /// Fill bounds
        /// </summary>
        public Rectangle FillBounds
        {
            get
            {
                return _fillImage.Bounds;
            }
        }

        #endregion Public section

        private void _fillImage_Click(object sender, System.EventArgs e)
        {

        }
    }
}
