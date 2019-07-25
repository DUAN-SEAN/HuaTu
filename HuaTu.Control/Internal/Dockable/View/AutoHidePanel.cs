using HuaTu.Controls.Public.Dockable;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HuaTu.Controls.Internal.Dockable.View
{
    /// <summary>
    /// Implementation of autohide panel
    /// </summary>
    internal partial class AutoHidePanel : Control
    {
        #region Fields

        private DockingContainer _restoreParent = null;
        private CommandHandler _autoHideHandler = null;
        private CommandHandler _autoShowHandler = null;

        #endregion Fields

        #region Instance

        /// <summary>
        /// Default constructor
        /// </summary>
        public AutoHidePanel()
        {
        }

        #endregion Instance

        #region Public section

        /// <summary>
        /// Accessor of the text
        /// </summary>
        public override string Text
        {
            get
            {
                if (View != null)
                {
                    return View.Text;
                }

                return string.Empty;
            }
            set
            {
                if (View != null)
                {
                    View.Text = value;
                }
            }
        }

        /// <summary>
        /// Accessor of the icon
        /// </summary>
        public Icon Icon
        {
            get
            {
                if (View != null)
                {
                    return View.Icon;
                }

                return null;
            }
            set
            {
                if (View != null)
                {
                    View.Icon = value;
                }
            }
        }


        /// <summary>
        /// Restore panel
        /// </summary>
        public DockingContainer RestoreParent
        {
            get { return _restoreParent; }
            set { _restoreParent = value; }
        }


        /// <summary>
        /// command
        /// </summary>
        public CommandHandler AutoHideHandler
        {
            get { return _autoHideHandler; }
            set { _autoHideHandler = value; }
        }

        /// <summary>
        /// command
        /// </summary>
        public CommandHandler AutoShowHandler
        {
            get { return _autoShowHandler; }
            set { _autoShowHandler = value; }
        }


        /// <summary>
        /// Accessor of the selected form
        /// </summary>
        public FormsTabbedView View
        {
            get
            {
                if (Controls.Count > 0)
                {
                    return Controls[0] as FormsTabbedView;
                }

                return null;
            }
        }

        #endregion Public section

        #region Protected section

        /// <summary>
        /// Occurs when a control was added to collection
        /// </summary>
        /// <param name="e"></param>
        protected override void OnControlAdded(ControlEventArgs e)
        {
            e.Control.TextChanged += OnViewTextChanged;
            OnTextChanged(e);

            base.OnControlAdded(e);
        }

        /// <summary>
        /// Occurs when control was removed
        /// </summary>
        /// <param name="e"></param>
        protected override void OnControlRemoved(ControlEventArgs e)
        {
            e.Control.TextChanged -= OnViewTextChanged;

            base.OnControlRemoved(e);
        }

        /// <summary>
        /// Occurs when view text is changed
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">event args</param>
        private void OnViewTextChanged(object sender, EventArgs e)
        {
            OnTextChanged(e);
        }

        #endregion Protected section
    }
}
