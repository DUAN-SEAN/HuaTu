using HuaTu.Controls.Internal.Dockable.Data;
using HuaTu.Controls.Public.Dockable.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InvalidateEventArgs = HuaTu.Controls.Internal.Dockable.Data.InvalidateEventArgs;

namespace HuaTu.Controls.Internal.Dockable.View
{
    /// <summary>
    /// Preview pane
    /// </summary>
    internal sealed partial class PreviewPane : OwnerDrawPanel
    {
        #region Fields

        private PreviewRenderer _renderer = null;

        #endregion Fields

        #region Instance

        /// <summary>
        /// Default constructor
        /// </summary>
        public PreviewPane()
        {
            Renderer = new PreviewRenderer();
        }

        #endregion Instance

        #region Public section

        /// <summary>
        /// Accessor of the renderer
        /// </summary>
        public PreviewRenderer Renderer
        {
            get { return _renderer; }
            set
            {
                if (_renderer != null)
                {
                    _renderer.Invalidated -= OnRendererInvalidated;
                }

                _renderer = value;

                if (_renderer != null)
                {
                    _renderer.Invalidated += OnRendererInvalidated;
                }
            }
        }

        #endregion Public section

        #region Protected section

        /// <summary>
        /// Occurs when visibility of this control changes
        /// </summary>
        /// <param name="e">event argument</param>
        protected override void OnSizeChanged(EventArgs e)
        {
            using (GraphicsPath path = GraphicsOrder.CreateRoundRectPath(0, 0, Width, Height, 5))
            {
                Region = new Region(ClientRectangle);
                Region = new Region(path);
            }

            base.OnSizeChanged(e);

            Invalidate();
        }

        /// <summary>
        /// Paint
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (Renderer != null)
            {
                PreviewRenderer.ShowFormPreview(ClientRectangle, Renderer, e.Graphics);
            }
        }

        #endregion Protected section

        #region Private section

        /// <summary>
        /// Occurs when renderer is invalidated
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">event arguments</param>
        private void OnRendererInvalidated(object sender, InvalidateEventArgs e)
        {
            if (e.Bounds.IsEmpty)
            {
                Invalidate();
            }
            else
            {
                Invalidate(e.Bounds);
            }
        }

        #endregion Private section
    }
}
