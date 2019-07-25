using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HuaTu.Controls.Internal.Dockable.View
{
    /// <summary>
    /// Owner draw panel
    /// </summary>
    internal partial class OwnerDrawPanel : Panel
    {
        #region Instance.

        /// <summary>
        /// Default constructor
        /// </summary>
        public OwnerDrawPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Selectable, true);
            SetStyle(ControlStyles.UserPaint, true);
        }

        #endregion Instance.
    }
}
