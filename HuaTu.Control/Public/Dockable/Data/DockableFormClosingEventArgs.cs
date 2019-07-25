using System;
using System.Windows.Forms;

namespace HuaTu.Controls.Public.Dockable.Data
{
    /// <summary>
    /// Form closing event args
    /// </summary>
    public class DockableFormClosingEventArgs : FormEventArgs
    {
        #region Fields

        private bool _cancel = false;

        #endregion Fields

        #region Instance

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="form">form</param>
        /// <param name="formId">form identifier</param>
        public DockableFormClosingEventArgs(Form form, Guid formId) : base(form, formId)
        {
        }

        #endregion Instance

        #region Public section

        /// <summary>
        /// Flag indicating if should cancel the form closing
        /// </summary>
        public bool Cancel
        {
            get { return _cancel; }
            set { _cancel = value; }
        }

        #endregion Public section
    }
}