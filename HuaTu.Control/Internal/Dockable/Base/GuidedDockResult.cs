using HuaTu.Controls.Public.Dockable.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HuaTu.Controls.Internal.Dockable.Base
{
    /// <summary>
    /// Result of guided dock
    /// </summary>
    internal class GuidedDockResult
    {
        #region Fields

        private DockStyle _dock = DockStyle.None;
        private DockableMode _dockMode = DockableMode.Outer;

        #endregion Fields

        #region Instance

        /// <summary>
        /// Default constructor
        /// </summary>
        public GuidedDockResult()
        {
        }

        #endregion Instance

        #region Public section

        /// <summary>
        /// Accessor for dock result
        /// </summary>
        public DockStyle Dock
        {
            get { return _dock; }
            set { _dock = value; }
        }

        /// <summary>
        /// Accessor for dock mode
        /// </summary>
        public DockableMode DockMode
        {
            get { return _dockMode; }
            set { _dockMode = value; }
        }

        #endregion Public section
    }
}
