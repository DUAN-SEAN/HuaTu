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
    /// Control collection for forms decorator
    /// </summary>
    internal class FormsDecoratorControlCollection : Control.ControlCollection
    {
        #region Fields

        private FormsContainer _formsPanel = new FormsContainer();
        private Control _titleBar = new OwnerDrawPanel();
        private Control _topMargin = new Control();
        private Control _leftMargin = new Control();
        private Control _rightMargin = new Control();
        private Control _bottomMargin = new Control();

        #endregion Fields

        #region Instance

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="owner">owner control</param>
        public FormsDecoratorControlCollection(Control owner) : base(owner)
        {
            base.Add(_leftMargin);
            base.Add(_rightMargin);
            base.Add(_titleBar);
            base.Add(_topMargin);
            base.Add(_bottomMargin);
            base.Add(_formsPanel);
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
        public FormsContainer FormsPanel
        {
            get { return _formsPanel; }
        }

        /// <summary>
        /// Accessor of the forms title bar
        /// </summary>
        public Control TitleBar
        {
            get { return _titleBar; }
        }

        /// <summary>
        /// Accessor of the forms top margin
        /// </summary>
        public Control TopMargin
        {
            get { return _topMargin; }
        }

        /// <summary>
        /// Accessor of the forms left margin
        /// </summary>
        public Control LeftMargin
        {
            get { return _leftMargin; }
        }

        /// <summary>
        /// Accessor of the forms right margin
        /// </summary>
        public Control RightMargin
        {
            get { return _rightMargin; }
        }

        /// <summary>
        /// Accessor of the forms bottom margin
        /// </summary>
        public Control BottomMargin
        {
            get { return _bottomMargin; }
        }

        #endregion Public section
    }
}
