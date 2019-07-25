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
    internal class ControlSwitchedEventArgs : EventArgs
    {
        #region Fields

        private Control _oldControl = null;
        private Control _newControl = null;

        #endregion Fields

        #region Instance

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="oldControl">old control</param>
        /// <param name="newControl">new control</param>
        public ControlSwitchedEventArgs(Control oldControl, Control newControl)
        {
            _oldControl = oldControl;
            _newControl = newControl;
        }

        #endregion Instance

        #region Public section

        /// <summary>
        /// Accessor of the old control
        /// </summary>
        public Control OldControl
        {
            get { return _oldControl; }
        }

        /// <summary>
        /// Accessor of the new control
        /// </summary>
        public Control NewControl
        {
            get { return _newControl; }
        }

        #endregion Public section
    }
}
