using HuaTu.Controls.Public.Dockable.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HuaTu.Controls.Internal.Dockable.Data
{
    /// <summary>
    /// Preview event args
    /// </summary>
    internal class PreviewEventArgs : FormEventArgs
    {
        #region Fields

        private Point _buttonLocation = new Point();

        #endregion Fields

        #region Instance

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="buttonLocation">button location</param>
        /// <param name="form">form</param>
        public PreviewEventArgs(Point buttonLocation, Form form) : base(form, Guid.Empty)
        {
            _buttonLocation = buttonLocation;
        }

        #endregion Instance

        #region Public section

        /// <summary>
        /// Accessor of button location
        /// </summary>
        public Point ButtonLocation
        {
            get { return _buttonLocation; }
        }

        #endregion Public section
    }
}
