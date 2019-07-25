using HuaTu.Controls.Internal.Dockable.Collection;
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
    /// Forms container control
    /// </summary>
    internal partial class FormsContainer : Control
    {
        #region Instance

        /// <summary>
        /// Default constructor
        /// </summary>
        public FormsContainer()
        {
        }

        #endregion Instance

        #region Protected section

        /// <summary>
        /// Creates the control collection instance
        /// </summary>
        /// <returns>collection</returns>
        protected override ControlCollection CreateControlsInstance()
        {
            return new FormsContainerControlCollection(this);
        }

        #endregion Protected section
    }
}
