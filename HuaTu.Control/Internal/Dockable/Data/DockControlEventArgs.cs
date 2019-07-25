using HuaTu.Controls.Public.Dockable.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HuaTu.Controls.Internal.Dockable.Data
{
    /// <summary>
    /// Control changed event args
    /// </summary>
    internal class DockControlEventArgs : EventArgs
    {
        #region Fields

        private Control _control = null;
        private DockStyle _dock = DockStyle.None;
        private DockableMode _dockMode = DockableMode.Outer;

        #endregion Fields

        #region Instance

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="control">control</param>
        /// <param name="dock">dock</param>
        /// <param name="mode">dock mode</param>
        public DockControlEventArgs(Control control, DockStyle dock, DockableMode mode)
        {
            _control = control;
            _dock = dock;
            _dockMode = mode;
        }

        #endregion Instance

        #region Public section

        /// <summary>
        /// Accessor of the control
        /// </summary>
        public Control Control
        {
            get { return _control; }
        }

        /// <summary>
        /// Accessor for dock result
        /// </summary>
        public DockStyle Dock
        {
            get { return _dock; }
        }

        /// <summary>
        /// Accessor for dock mode
        /// </summary>
        public DockableMode DockMode
        {
            get { return _dockMode; }
        }

        #endregion Public section
    }
}
