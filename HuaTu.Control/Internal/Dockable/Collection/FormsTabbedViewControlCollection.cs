using HuaTu.Controls.Internal.Dockable.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HuaTu.Controls.Internal.Dockable.Collection
{
    /// <summary>
    /// Control collection for forms tabbed view
    /// </summary>
    internal class FormsTabbedViewControlCollection : Control.ControlCollection
    {
        #region Fields

        private FormsDecorator _pagesPanel = new FormsDecorator();

        #endregion Fields

        #region Instance

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="owner">owner control</param>
        public FormsTabbedViewControlCollection(Control owner) : base(owner)
        {
            base.Add(_pagesPanel);
        }

        #endregion Instance

        #region Public section

        /// <summary>
        /// Add a control to the collection
        /// </summary>
        /// <param name="value">new control added</param>
        public override void Add(Control value)
        {
            // Disconnect from the base
            throw new NotSupportedException();
        }

        /// <summary>
        /// Add a range of controls
        /// </summary>
        /// <param name="controls">range of controls</param>
        public override void AddRange(Control[] controls)
        {
            // Disconnect from the base
            throw new NotSupportedException();
        }

        /// <summary>
        /// Clears the controls collection
        /// </summary>
        public override void Clear()
        {
            // Disconnect from the base
            throw new NotSupportedException();
        }

        /// <summary>
        /// Remove a control
        /// </summary>
        /// <param name="value">control to be removed</param>
        public override void Remove(Control value)
        {
            // Disconnect from the base
            throw new NotSupportedException();
        }

        /// <summary>
        /// Remove by key
        /// </summary>
        /// <param name="key">key</param>
        public override void RemoveByKey(string key)
        {
            // Disconnect from the base
            throw new NotSupportedException();
        }

        /// <summary>
        /// Set child index
        /// </summary>
        /// <param name="child">child control</param>
        /// <param name="newIndex">zero based new child index</param>
        public override void SetChildIndex(Control child, int newIndex)
        {
            // Disconnect from the base
            throw new NotSupportedException();
        }

        /// <summary>
        /// Accessor of the forms panel
        /// </summary>
        public FormsDecorator PagesPanel
        {
            get { return _pagesPanel; }
        }

        #endregion Public section
    }
}
