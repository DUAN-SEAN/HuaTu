using HuaTu.Controls.Internal.Dockable.View;
using HuaTu.Controls.Public.Dockable;
using HuaTu.Controls.Public.Dockable.Base;
using HuaTu.Controls.Public.Dockable.Enum;
using HuaTu.Controls.Public.UnitPagePackage.Drawer;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace HuaTu.Controls.Internal.Dockable.Base
{
    /// <summary>
    /// Manager for dock layout
    /// </summary>
    internal class DockLayout
    {
        #region Fields

        private const int MinAvailableSize = 64;

        private FormWrapper _host = null;
        private bool _canMoveByMouseFilledForms = true;

        #endregion Fields

        #region Instance

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="host">host window</param>
        public DockLayout(FormWrapper host)
        {
            _host = host;
        }

        #endregion Instance

        #region Public section
        #region Events

        /// <summary>
        /// Occurs when begin moving a view using the mouse
        /// </summary>
        public event EventHandler<ControlEventArgs> BeginMoveByMouse;

        /// <summary>
        /// Occurs when moving the window by mouse
        /// </summary>
        public event EventHandler MoveByMouse;

        /// <summary>
        /// Occurs when finished moving the window by mouse
        /// </summary>
        public event EventHandler EndMoveByMouse;

        /// <summary>
        /// Occurs when should show floating windows
        /// </summary>
        public event EventHandler ShowFloatingWindows;

        /// <summary>
        /// Occurs when should destroy forms tabbed view
        /// </summary>
        public event EventHandler<ControlEventArgs> DestroyFormsTabbedView;

        #endregion Events


        /// <summary>
        /// Flag indicating if the filled forms can be moved by mouse
        /// </summary>
        public bool CanMoveByMouseFilledForms
        {
            get { return _canMoveByMouseFilledForms; }
            set { _canMoveByMouseFilledForms = value; }
        }

        /// <summary>
        /// Create floating container for the view
        /// </summary>
        /// <param name="view">view for which to create floating container</param>
        /// <param name="bounds">bounds of the floating container</param>
        /// <returns>floating container</returns>
        public DockingContainer CreateFloatingContainer(FormsTabbedView view, Rectangle bounds)
        {
            DockingContainer container = new DockingContainer();
            _host.AddFirst(container);

            container.Bounds = bounds;

            container.SetModeSingleChild(view);

            if (EnumUtility.Contains(view.AllowedDock, DockAllowed.None))
            {
                SetViewDock(view, DockStyle.None, DockStyle.None, DockableMode.None);
            }
            else if (EnumUtility.Contains(view.AllowedDock, DockAllowed.Left))
            {
                DockControl(container, null, DockStyle.Left, DockableMode.Outer);
            }
            else if (EnumUtility.Contains(view.AllowedDock, DockAllowed.Right))
            {
                DockControl(container, null, DockStyle.Right, DockableMode.Outer);
            }
            else if (EnumUtility.Contains(view.AllowedDock, DockAllowed.Top))
            {
                DockControl(container, null, DockStyle.Top, DockableMode.Outer);
            }
            else if (EnumUtility.Contains(view.AllowedDock, DockAllowed.Bottom))
            {
                DockControl(container, null, DockStyle.Bottom, DockableMode.Outer);
            }
            else if (EnumUtility.Contains(view.AllowedDock, DockAllowed.Fill))
            {
                DockControl(container, null, DockStyle.Fill, DockableMode.Inner);
            }
            else
            {
                _host.Remove(container);
                throw new NotSupportedException("Err");   // invalid allowed dock
            }

            return container;
        }

        /// <summary>
        /// Dock a form previously added to guider
        /// </summary>
        /// <param name="containerToDock">container to dock</param>
        /// <param name="containerWhereToDock">container where to dock</param>
        /// <param name="mode">were to dock</param>
        public void DockControl(DockingContainer containerToDock, DockingContainer containerWhereToDock, DockStyle dock, DockableMode mode)
        {
            Debug.Assert(dock != DockStyle.None);

            if (ShouldDockInHost(containerToDock, containerWhereToDock))
            {
                DockInHost(containerToDock, dock, mode);
            }
            else if (ShouldDockInParentContainer(containerToDock, containerWhereToDock))
            {
                DockInParentContainer(containerToDock, containerWhereToDock, dock);
            }

            OnShowFloatingWindows();
        }

        /// <summary>
        /// Undock the view
        /// </summary>
        /// <param name="view">view</param>
        public void Undock(FormsTabbedView view)
        {
            Point mousePosition = _host.PointToClient(Control.MousePosition);

            DockingContainer container = (DockingContainer)view.Parent;
            Rectangle bounds = _host.GetBoundsInHost(container);

            ComputeFloatingBounds(mousePosition, view.FloatingSize, ref bounds);

            Undock(view, bounds);
        }

        /// <summary>
        /// Undock the view
        /// </summary>
        /// <param name="view">view</param>
        /// <param name="floatingBounds">floating bounds</param>
        public void Undock(FormsTabbedView view, Rectangle floatingBounds)
        {
            DockingContainer container = (DockingContainer)view.Parent;
            container.SetModeEmpty();

            RemoveContainer(container);

            CreateFloatingContainer(view, floatingBounds);
        }

        /// <summary>
        /// Set view dock
        /// </summary>
        /// <param name="view">view</param>
        /// <param name="hostContainerDock">host container dock</param>
        /// <param name="dock">dock</param>
        /// <param name="mode">mode</param>
        public void SetViewDock(FormsTabbedView view, DockStyle hostContainerDock, DockStyle dock, DockableMode mode)
        {
            UpdateViewButtons(view);

            if (CanMoveByMouseFilledForms == false && hostContainerDock == DockStyle.Fill)
            {
                view.CanMoveByMouse = false;
            }
            else
            {
                view.CanMoveByMouse = true;
            }

            if (view.HostContainerDock == dock)
            {
                view.SetDock(hostContainerDock, dock, mode);
                return;
            }

            view.SetDock(hostContainerDock, dock, mode);

            view.Positioner.BeginMoveByMouse -= OnPositionerBeginMoveByMouse;
            view.Positioner.MoveByMouse -= OnPositionerMoveByMouse;
            view.Positioner.EndMoveByMouse -= OnPositionerEndMoveByMouse;

            view.Positioner.Dispose();

            if (dock == DockStyle.None)
            {
                view.Positioner = new ControlPositioner(view.Parent);

                view.Positioner.MoveByMouse += OnPositionerMoveByMouse;
                view.Positioner.EndMoveByMouse += OnPositionerEndMoveByMouse;
            }
            else if (dock == Globals.DockAutoHide)
            {
                view.Positioner = null;
                return;
            }
            else
            {
                view.Positioner = new ControlPositioner(view);
            }

            view.Positioner.BeginMoveByMouse += OnPositionerBeginMoveByMouse;
        }

        #endregion Public section

        #region Private section
        #region Received events

        /// <summary>
        /// On begin move a positioner using the mouse
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">event arguments</param>
        private void OnPositionerBeginMoveByMouse(object sender, EventArgs e)
        {
            DockingContainer container = sender as DockingContainer;
            if (container != null)
            {
                Debug.Assert(_host.Contains(container), "Only floating forms should have a container as positioner. Docked forms should have their tabbed view.");
                _host.MoveFirst(container);
            }
            else
            {
                FormsTabbedView view = sender as FormsTabbedView;
                Debug.Assert(view != null, "The positioner should wrap either a container for floating forms or a tabbed view for docked forms");

                Undock(view);

                container = view.Parent as DockingContainer;
            }

            EventHandler<ControlEventArgs> handler = BeginMoveByMouse;
            if (handler != null)
            {
                ControlEventArgs args = new ControlEventArgs(container);
                handler(this, args);
            }
        }

        /// <summary>
        /// On moving a positioner using the mouse
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">event arguments</param>
        private void OnPositionerMoveByMouse(object sender, EventArgs e)
        {
            EventHandler handler = MoveByMouse;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// On end moving a positioner using the mouse
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">event arguments</param>
        private void OnPositionerEndMoveByMouse(object sender, EventArgs e)
        {
            EventHandler handler = EndMoveByMouse;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion Received events


        /// <summary>
        /// Show the floating windows
        /// </summary>
        private void OnShowFloatingWindows()
        {
            EventHandler handler = ShowFloatingWindows;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Destroy forms tabbed view
        /// </summary>
        /// <param name="view">tabbed view</param>
        private void OnDestroyFormsTabbedView(FormsTabbedView view)
        {
            EventHandler<ControlEventArgs> handler = DestroyFormsTabbedView;
            if (handler != null)
            {
                ControlEventArgs args = new ControlEventArgs(view);
                handler(this, args);
            }
        }


        /// <summary>
        /// Remove the container
        /// </summary>
        /// <param name="container">container to remove</param>
        private void RemoveContainer(DockingContainer container)
        {
            Debug.Assert(container.Parent != null, "Docked container must be hosted somewere");

            container.SetModeEmpty();

            DockingContainer containerParent = container.Parent as DockingContainer;
            if (containerParent == null)
            {
                Debug.Assert(container.Parent.Handle == _host.Handle, "Parents of FormsTabbedView should be only DockingContainer or _host");
            }
            else
            {
                DockingContainer otherContainer = containerParent.OtherPane(container);
                Debug.Assert(otherContainer != null, "Container in container means that parent container has contained two containers and a splitter");

                FormsTabbedView otherView = otherContainer.SingleChild;
                FormsTabbedView linkedView = otherContainer.LinkedView;

                if (otherView == null && linkedView == null)
                {
                    if (otherContainer.LeftPane != null)
                    {
                        DockingContainer leftPane = otherContainer.LeftPane;
                        DockingContainer rightPane = otherContainer.RightPane;

                        otherContainer.SetModeEmpty();

                        containerParent.SetModeHSplit(leftPane, rightPane);
                    }
                    else if (otherContainer.TopPane != null)
                    {
                        DockingContainer topPane = otherContainer.TopPane;
                        DockingContainer bottomPane = otherContainer.BottomPane;

                        otherContainer.SetModeEmpty();

                        containerParent.SetModeVSplit(topPane, bottomPane);
                    }
                }
                else
                {
                    Debug.Assert((otherView == null || linkedView == null), "Other container must have a view");

                    otherContainer.SetModeEmpty();

                    if (otherView != null)
                    {
                        containerParent.SetModeSingleChild(otherView);
                    }
                    else
                    {
                        containerParent.SetModeLinked(linkedView);
                        AutoHidePanel linkedPanel = (AutoHidePanel)linkedView.Parent;
                        linkedPanel.RestoreParent = containerParent;
                        Autohide.HideRestoreContainers(linkedPanel);
                    }

                    // If floating container inside host
                    if (containerParent.Parent.Handle == _host.Handle && containerParent.Dock == DockStyle.None)
                    {
                        Debug.Assert(linkedView == null, "Can't have linked view in floating container");
                        SetViewDock(otherView, DockStyle.None, DockStyle.None, DockableMode.None);
                    }
                }

                otherContainer.Parent = null;
                otherContainer.Splitter.Parent = null;
                otherContainer.Dispose();
            }

            container.Parent = null;
            container.Splitter.Parent = null;
            container.Dispose();
        }



        /// <summary>
        /// Check if should dock in host
        /// </summary>
        /// <param name="containerToDock">container to dock</param>
        /// <param name="containerWhereToDock">container where to dock</param>
        /// <returns>true if should dock in host</returns>
        private bool ShouldDockInHost(DockingContainer containerToDock, DockingContainer containerWhereToDock)
        {
            if (containerToDock == null)
            {
                return false;
            }

            if (containerWhereToDock != null)
            {
                if (containerWhereToDock.SingleChild != null)
                {
                    if (containerWhereToDock.SingleChild.HostContainerDock != DockStyle.None)
                    {
                        if (containerToDock.SingleChild != null)
                        {
                            if (containerWhereToDock.SingleChild.AllowedDock == containerToDock.SingleChild.AllowedDock)
                            {
                                return false;
                            }
                        }
                    }
                }
                else
                {
                    return false;
                }
            }

            // Can dock only containers containing single child tabbed view
            return containerToDock.SingleChild != null;
        }

        /// <summary>
        /// Check if should dock in parent container
        /// </summary>
        /// <param name="containerToDock">container to dock</param>
        /// <param name="containerWhereToDock">container where to dock</param>
        /// <returns>true if should dock in host</returns>
        private bool ShouldDockInParentContainer(DockingContainer containerToDock, DockingContainer containerWhereToDock)
        {
            if (containerToDock == null)
            {
                return false;
            }

            if (containerWhereToDock == null)
            {
                return false;
            }

            // Can dock only containers containing single child tabbed view and same allowed dock
            if (containerWhereToDock.SingleChild != null && containerToDock.SingleChild != null)
            {
                return containerToDock.SingleChild.AllowedDock == containerWhereToDock.SingleChild.AllowedDock;
            }

            return false;
        }


        /// <summary>
        /// Dock the form in host
        /// </summary>
        /// <param name="containerToDock">container to dock</param>
        /// <param name="dock">dock</param>
        /// <param name="mode">mode</param>
        private void DockInHost(DockingContainer container, DockStyle dock, DockableMode mode)
        {
            Rectangle availableBounds = FormWrapper.GetFillScreenRectangle(_host);

            container.SingleChild.SaveFloatingSize();
            container.Splitter.Visible = false;

            if (_host.Contains(container.Splitter) == false && dock != DockStyle.Fill)
            {
                _host.AddLast(container.Splitter);
            }

            if (mode == DockableMode.Inner)
            {
                if (dock != DockStyle.Fill)
                {
                    _host.MoveFirst(container);
                    _host.MoveFirst(container.Splitter);
                }
            }
            else
            {
                if (dock != DockStyle.Fill)
                {
                    _host.MoveLast(container.Splitter);
                    _host.MoveLast(container);
                }
            }

            container.Dock = dock;

            if (dock != DockStyle.Fill)
            {
                container.Splitter.Dock = dock;
                container.Splitter.Visible = true;
            }
            else
            {
                container.SingleChild.ButtonsRenderer = new TopUnitButtonDrawer();
            }

            // Must call this again after splitter is docked and made visible, to have proper splitter.
            // Must have this if/else block also before making the splitter visible to prevent flickering
            if (mode == DockableMode.Inner)
            {
                if (dock != DockStyle.Fill)
                {
                    _host.MoveFirst(container);
                    _host.MoveFirst(container.Splitter);

                    if (availableBounds.IsEmpty == false)
                    {
                        if (dock == DockStyle.Left || dock == DockStyle.Right)
                        {
                            if (container.Width >= availableBounds.Width - MinAvailableSize)
                            {
                                container.Width = availableBounds.Width - MinAvailableSize;
                            }
                        }
                        else if (dock == DockStyle.Top || dock == DockStyle.Bottom)
                        {
                            if (container.Height >= availableBounds.Height - MinAvailableSize)
                            {
                                container.Height = availableBounds.Height - MinAvailableSize;
                            }
                        }
                    }
                }
            }
            else
            {
                if (dock != DockStyle.Fill)
                {
                    _host.MoveLast(container.Splitter);
                    _host.MoveLast(container);
                }
            }

            SetViewDock(container.SingleChild, dock, dock, mode);
        }

        /// <summary>
        /// Dock in parent container
        /// </summary>
        /// <param name="containerToDock">container to dock</param>
        /// <param name="containerWhereToDock">container where to dock</param>
        /// <param name="dock">dock</param>
        private void DockInParentContainer(DockingContainer containerToDock, DockingContainer containerWhereToDock, DockStyle dock)
        {
            FormsTabbedView formTabbedView = containerToDock.SingleChild;
            FormsTabbedView parentTabbedView = containerWhereToDock.SingleChild;

            containerToDock.SingleChild.SaveFloatingSize();
            containerToDock.Splitter.Visible = false;
            containerToDock.SetModeEmpty();

            if (dock != DockStyle.Fill)
            {
                containerWhereToDock.SetModeEmpty();
            }

            RemoveContainer(containerToDock);

            DockStyle parentDock = DockStyle.None;
            switch (dock)
            {
                case DockStyle.Left:
                    SplitHorizontally(formTabbedView, parentTabbedView, containerWhereToDock);
                    parentDock = DockStyle.Right;
                    break;

                case DockStyle.Right:
                    SplitHorizontally(parentTabbedView, formTabbedView, containerWhereToDock);
                    parentDock = DockStyle.Left;
                    break;

                case DockStyle.Top:
                    SplitVertically(formTabbedView, parentTabbedView, containerWhereToDock);
                    parentDock = DockStyle.Bottom;
                    break;

                case DockStyle.Bottom:
                    SplitVertically(parentTabbedView, formTabbedView, containerWhereToDock);
                    parentDock = DockStyle.Top;
                    break;

                case DockStyle.Fill:
                    parentDock = parentTabbedView.HostContainerDock;
                    DockFillInParentContainer(formTabbedView, parentTabbedView);
                    OnDestroyFormsTabbedView(formTabbedView);
                    break;

                default:
                    throw new InvalidOperationException();
            }

            if (parentTabbedView.HostContainerDock != DockStyle.None)
            {  // Keep the parent logical dock if already docked
                if (parentDock != parentTabbedView.CurrentDock)
                {
                    SetViewDock(parentTabbedView, parentTabbedView.HostContainerDock, parentDock, parentTabbedView.CurrentDockMode);
                }
                parentDock = parentTabbedView.HostContainerDock;
            }

            UpdateViewButtons(parentTabbedView);

            if (dock != DockStyle.Fill)
            {
                SetViewDock(formTabbedView, parentDock, dock, parentTabbedView.CurrentDockMode);
            }
        }


        /// <summary>
        /// Split two forms in horizontal splitted view
        /// </summary>
        /// <param name="leftFormTabbedView">view to dock left</param>
        /// <param name="rightFormTabbedView">view to dock right</param>
        /// <param name="parentContainer">parent container which will host the two forms</param>
        private static void SplitHorizontally(FormsTabbedView leftFormTabbedView, FormsTabbedView rightFormTabbedView, DockingContainer parentContainer)
        {
            DockingContainer newRightFormContainer = new DockingContainer();
            DockingContainer newLeftFormContainer = new DockingContainer();

            parentContainer.SetModeHSplit(newLeftFormContainer, newRightFormContainer);

            newRightFormContainer.SetModeSingleChild(rightFormTabbedView);
            newLeftFormContainer.SetModeSingleChild(leftFormTabbedView);
        }

        /// <summary>
        /// Split two forms in vertically splitted view
        /// </summary>
        /// <param name="topFormTabbedView">view to dock top</param>
        /// <param name="bottomFormTabbedView">view to dock bottom</param>
        /// <param name="parentContainer">parent container which will host the two forms</param>
        private static void SplitVertically(FormsTabbedView topFormTabbedView, FormsTabbedView bottomFormTabbedView, DockingContainer parentContainer)
        {
            DockingContainer newBottomFormContainer = new DockingContainer();
            DockingContainer newTopFormContainer = new DockingContainer();

            parentContainer.SetModeVSplit(newTopFormContainer, newBottomFormContainer);

            newBottomFormContainer.SetModeSingleChild(bottomFormTabbedView);
            newTopFormContainer.SetModeSingleChild(topFormTabbedView);
        }

        /// <summary>
        /// Dock fill in parent container
        /// </summary>
        /// <param name="formTabbedView">view to dock</param>
        /// <param name="parentTabbedView">view where will dock</param>
        private static void DockFillInParentContainer(FormsTabbedView formTabbedView, FormsTabbedView parentTabbedView)
        {
            Form selected = formTabbedView.GetPageAt(formTabbedView.SelectedIndex);
            DockableFormInfo[] movedPages = formTabbedView.MovePagesTo(parentTabbedView);

            foreach (DockableFormInfo movedPage in movedPages)
            {
                movedPage.Dock = DockStyle.Fill;
                movedPage.DockMode = DockableMode.Inner;
            }

            for (int index = 0; index < parentTabbedView.Count; index++)
            {
                if (parentTabbedView.GetPageAt(index) == selected)
                {
                    parentTabbedView.SelectedIndex = index;
                    break;
                }
            }
        }

        /// <summary>
        /// Compute floating bounds
        /// </summary>
        /// <param name="mousePosition">mouse position</param>
        /// <param name="floatingSize">floating size</param>
        /// <param name="bounds">floating bounds</param>
        private static void ComputeFloatingBounds(Point mousePosition, Size floatingSize, ref Rectangle bounds)
        {
            bounds.Size = floatingSize;
            if (bounds.Contains(mousePosition) == false)
            {
                bounds.X = mousePosition.X - bounds.Width / 2;
            }
        }

        /// <summary>
        /// Update view buttons
        /// </summary>
        /// <param name="view">view</param>
        private static void UpdateViewButtons(FormsTabbedView view)
        {
            if (view.HostContainerDock == DockStyle.Fill)
            {
                if (view.ButtonsRenderer.GetType() != typeof(TopUnitButtonDrawer))
                {
                    view.ButtonsRenderer = new TopUnitButtonDrawer();
                }
            }
            else if (view.ButtonsRenderer.GetType() != typeof(BottomUnitButtonDrawer))
            {
                view.ButtonsRenderer = new BottomUnitButtonDrawer();
            }
        }

        #endregion Private section
    }
}