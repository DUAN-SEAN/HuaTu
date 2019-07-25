using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuaTu.Controls.Internal.Dockable.Data
{
    /// <summary>
    /// Invalidate event args
    /// </summary>
    internal class InvalidateEventArgs : EventArgs
    {
        #region Fields

        private Rectangle _bounds = new Rectangle();

        #endregion Fields

        #region Instance

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bounds">bounds invalidated</param>
        public InvalidateEventArgs(Rectangle bounds)
        {
            _bounds = bounds;
        }

        #endregion Instance

        #region Public section

        /// <summary>
        /// Accessor of the invalidated bounds
        /// </summary>
        public Rectangle Bounds
        {
            get { return _bounds; }
        }

        #endregion Public section
    }
}
