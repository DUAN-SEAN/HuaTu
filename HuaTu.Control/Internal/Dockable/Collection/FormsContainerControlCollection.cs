using HuaTu.Controls.Internal.Dockable.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HuaTu.Controls.Internal.Dockable.Collection
{
    /// <summary>
    /// Control collection for forms container
    /// </summary>
    internal class FormsContainerControlCollection : Control.ControlCollection
    {
        #region Fields

        #endregion Fields

        #region Instance

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="owner">owner control</param>
        public FormsContainerControlCollection(Control owner) : base(owner)
        {
        }

        #endregion Instance

        #region Public section

        /// <summary>
        /// Occurs when top control was changed
        /// </summary>
        public event EventHandler<ControlSwitchedEventArgs> TopControlChanged;

        /// <summary>
        /// Add a control to the collection
        /// </summary>
        /// <param name="value">new control added</param>
        public override void Add(Control value)
        {
            Control top = TopControl;

            base.Add(value);

            if (TopControl != top)
            {
                OnTopControlChnaged(top, TopControl);
            }
        }

        /// <summary>
        /// Add a range of controls
        /// </summary>
        /// <param name="controls">range of controls</param>
        public override void AddRange(Control[] controls)
        {
            Control top = TopControl;

            base.AddRange(controls);

            if (TopControl != top)
            {
                OnTopControlChnaged(top, TopControl);
            }
        }

        /// <summary>
        /// Clears the controls collection
        /// </summary>
        public override void Clear()
        {
            Control top = TopControl;

            base.Clear();

            if (TopControl != top)
            {
                OnTopControlChnaged(top, TopControl);
            }
        }

        /// <summary>
        /// Remove a control
        /// </summary>
        /// <param name="value">control to be removed</param>
        public override void Remove(Control value)
        {
            Control top = TopControl;

            base.Remove(value);

            if (TopControl != top)
            {
                OnTopControlChnaged(top, TopControl);
            }
        }

        /// <summary>
        /// Remove by key
        /// </summary>
        /// <param name="key">key</param>
        public override void RemoveByKey(string key)
        {
            Control top = TopControl;

            base.RemoveByKey(key);

            if (TopControl != top)
            {
                OnTopControlChnaged(top, TopControl);
            }
        }

        /// <summary>
        /// Set child index
        /// </summary>
        /// <param name="child">child control</param>
        /// <param name="newIndex">zero based new child index</param>
        public override void SetChildIndex(Control child, int newIndex)
        {
            Control top = TopControl;

            base.SetChildIndex(child, newIndex);

            if (TopControl != top)
            {
                OnTopControlChnaged(top, TopControl);
            }
        }

        /// <summary>
        /// Accessor of the top control
        /// </summary>
        public Control TopControl
        {
            get
            {
                if (Count == 0)
                {
                    return null;
                }

                return this[0];
            }
        }

        #endregion Public section

        #region Private section

        /// <summary>
        /// Raises top control changed event
        /// </summary>
        /// <param name="oldControl">old control</param>
        /// <param name="newControl">new control</param>
        private void OnTopControlChnaged(Control oldControl, Control newControl)
        {
            EventHandler<ControlSwitchedEventArgs> handler = TopControlChanged;
            if (handler != null)
            {
                ControlSwitchedEventArgs args = new ControlSwitchedEventArgs(oldControl, newControl);
                handler(this, args);
            }
        }

        #endregion Private section
    }
}
