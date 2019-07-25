using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HuaTu.Controls.Public.Dockable.Data
{
    /// <summary>
    /// Form context menu event args
    /// </summary>
    public class FormContextMenuEventArgs : FormEventArgs
    {
        #region Fields

        private Point _menuLocation = new Point();

        #endregion Fields

        #region Instance

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="form">form</param>
        /// <param name="formId">form identifier</param>
        /// <param name="menuLocation">menu location relative to form</param>
        public FormContextMenuEventArgs(Point menuLocation, Form form, Guid formId) : base(form, formId)
        {
            _menuLocation = menuLocation;
        }

        #endregion Instance

        #region Public section

        /// <summary>
        /// Accessor of the form menu location
        /// </summary>
        public Point MenuLocation
        {
            get { return _menuLocation; }
        }

        #endregion Public section
    }
}
