using System;
using System.Windows.Forms;

namespace HuaTu.Controls.Public.Dockable.Data
{
    /// <summary>
    /// Form event args
    /// </summary>
    public class FormEventArgs : EventArgs
    {
        #region Fields

        private Form _form = null;
        private Guid _formId = Guid.Empty;

        #endregion Fields

        #region Instance

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="form">form</param>
        /// <param name="formId">form identifier</param>
        public FormEventArgs(Form form, Guid formId)
        {
            _form = form;
            _formId = formId;
        }

        #endregion Instance

        #region Public section

        /// <summary>
        /// Accessor of the form
        /// </summary>
        public Form Form
        {
            get { return _form; }
        }

        /// <summary>
        /// Accessor of the form identifier
        /// </summary>
        public Guid FormId
        {
            get { return _formId; }
        }

        #endregion Public section
    }
}