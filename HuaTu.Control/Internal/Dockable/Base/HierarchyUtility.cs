using HuaTu.Controls.Internal.Dockable.View;
using HuaTu.Controls.Public.Dockable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HuaTu.Controls.Internal.Dockable.Base
{
    internal class HierarchyUtility
    {
        #region Public section

        /// <summary>
        /// Get forms container
        /// </summary>
        /// <param name="form">form</param>
        /// <returns>forms container</returns>
        public static FormsContainer GetFormsContainer(Form form)
        {
            if (form == null)
            {
                return null;
            }

            return (FormsContainer)form.Parent;
        }

        /// <summary>
        /// Get form decorator
        /// </summary>
        /// <param name="form">form</param>
        /// <returns>form decorator</returns>
        public static FormsDecorator GetFormsDecorator(Form form)
        {
            FormsContainer container = GetFormsContainer(form);
            if (container == null)
            {
                return null;
            }
            return (FormsDecorator)container.Parent;
        }

        /// <summary>
        /// Get tabbed host
        /// </summary>
        /// <param name="form">form</param>
        /// <returns>tabbed host</returns>
        public static FormsTabbedView GetTabbedView(Form form)
        {
            FormsDecorator decorator = GetFormsDecorator(form);
            if (decorator == null)
            {
                return null;
            }

            return (FormsTabbedView)decorator.Parent;
        }

        /// <summary>
        /// Get the closest dockable container (first parent of given form which is dockable container)
        /// </summary>
        /// <param name="form">form</param>
        /// <returns>dockable container</returns>
        public static DockingContainer GetClosestDockableContainer(Form form)
        {
            if (form == null)
            {
                return null;
            }

            FormsTabbedView tabbedView = GetTabbedView(form);

            if (tabbedView.IsAutoHideMode)
            {
                AutoHidePanel panel = (AutoHidePanel)tabbedView.Parent;
                return panel.RestoreParent;
            }

            return (DockingContainer)tabbedView.Parent;
        }

        #endregion Public section
    }
}
