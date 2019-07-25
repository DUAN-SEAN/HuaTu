using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuaTu.Controls.Internal.Dockable.Data
{
    /// <summary>
    /// Template event args
    /// </summary>
    /// <typeparam name="T">args</typeparam>
    internal class TemplateEventArgs<T> : EventArgs
    {
        #region Fields

        private T _data = default(T);

        #endregion Fields

        #region Instance

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data">data</param>
        public TemplateEventArgs(T data)
        {
            _data = data;
        }

        #endregion Instance

        #region Public section

        /// <summary>
        /// Data accessor
        /// </summary>
        public T Data
        {
            get { return _data; }
        }

        #endregion Public section
    }
}
