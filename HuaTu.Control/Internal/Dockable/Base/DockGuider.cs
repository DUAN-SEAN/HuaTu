using HuaTu.Controls.Internal.Dockable.Data;
using HuaTu.Controls.Internal.Dockable.View;
using HuaTu.Controls.Public.Dockable;
using HuaTu.Controls.Public.Dockable.Enum;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HuaTu.Controls.Internal.Dockable.Base
{
    /// <summary>
    /// Dock guider
    /// </summary>
    internal class DockGuider : DisposablePage
    {
        #region Fields

        private FormWrapper _host = null;
        private DockGuiderWrapper _guider = null;
        private Control _movedWindow = null;
        private DockAllowed _allowedDock = DockAllowed.None;

        #endregion Fields

        #region Instance

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="host">host window</param>
        public DockGuider(FormWrapper host)
        {
            _host = host;
            _host.SizeChanged += OnHostSizeChanged;
            _host.Move += OnHostMoved;
            _host.VisibleChanged += OnHostVisibleChanged;

            _guider = new DockGuiderWrapper(_host);
        }

        #endregion Instance

        #region Public section

        /// <summary>
        /// Event raised when apply dock is requested
        /// </summary>
        public event EventHandler<DockControlEventArgs> ApplyDock;

        /// <summary>
        /// Begin the movement of a window
        /// </summary>
        /// <param name="window">moved window</param>
        /// <param name="allowedDock">allowed dock for the window</param>
        public void BeginWindowMovement(Control window, DockAllowed allowedDock)
        {
            if (_movedWindow != null)
            {
                throw new InvalidOperationException("Err001");
            }

            _movedWindow = window;
            _allowedDock = allowedDock;

            GuideForm();
        }

        /// <summary>
        /// Move a window using mouse
        /// </summary>
        public void MoveWindowByMouse()
        {
            if (_movedWindow == null)
            {
                return;
            }

            DockAllowed allowedDock = _allowedDock;
            Point screenLocation = Control.MousePosition;
            DockingContainer containerUnderMouse = GetContainerUnderMouse(screenLocation);

            Rectangle fillRectangle = FormWrapper.GetFillRectangleFromPoint(screenLocation, containerUnderMouse, _host);

            if (fillRectangle.IsEmpty)
            {
                _guider.HideCenterGuider();
            }
            else
            {
                if (containerUnderMouse == null)
                {
                    _guider.ShowCenterGuider(allowedDock, fillRectangle);
                }
                else
                {
                    allowedDock = DockAllowed.All;
                    _guider.ShowCenterGuider(allowedDock, fillRectangle);
                }
            }


            GuidedDockResult result = _guider.GetDockResult(allowedDock, screenLocation);

            if (result.DockMode== DockableMode.Outer && result.Dock != DockStyle.None)
            {
                Rectangle bounds = OuterDockPreviewEngine.GetPreviewBounds(result.Dock, _host, _movedWindow);
                _guider.ShowPreviewPanel(bounds);
            }
            else if (result.DockMode == DockableMode.Inner && result.Dock != DockStyle.None)
            {
                Rectangle freeBounds = FormWrapper.GetFillScreenRectangle(_host);
                Rectangle bounds = InnerDockPreviewEngine.GetPreviewBounds(result.Dock, _movedWindow, containerUnderMouse, freeBounds);
                if (bounds.IsEmpty)
                {
                    _guider.HidePreviewPanel();
                }
                else
                {
                    _guider.ShowPreviewPanel(bounds);
                }
            }
            else
            {
                _guider.HidePreviewPanel();
            }
        }

        /// <summary>
        /// Get leaf from point
        /// </summary>
        /// <param name="screenLocation">screen location</param>
        /// <param name="except">control excepted from search</param>
        /// <returns>leaf</returns>
        public DockingContainer GetLeafDockedContainerFromPoint(Point screenLocation, Control except)
        {
            return _host.GetLeafDockedContainerFromPoint(screenLocation, except);
        }

        /// <summary>
        /// End window movement
        /// </summary>
        public void EndWindowMovement()
        {
            if (_movedWindow == null)
            {
                return;
            }

            Point screenLocation = Control.MousePosition;
            DockAllowed allowedDock = _allowedDock;
            if (GetContainerUnderMouse(screenLocation) != null)
            {
                allowedDock = DockAllowed.All;
            }

            GuidedDockResult result = _guider.GetDockResult(allowedDock, screenLocation);
            Control movedControl = _movedWindow;

            StopMovement();

            _movedWindow = null;

            EventHandler<DockControlEventArgs> handler = ApplyDock;
            if (handler != null && result.Dock != DockStyle.None)
            {
                DockControlEventArgs args = new DockControlEventArgs(movedControl, result.Dock, result.DockMode);
                handler(this, args);
            }
        }

        /// <summary>
        /// Cancel the window movement
        /// </summary>
        /// <param name="window">moved window must be same as provided in BeginWindowMovement</param>
        public void CancelWindowMovement()
        {
            StopMovement();

            _movedWindow = null;
        }

        #endregion Public section

        #region Protected section

        /// <summary>
        /// Dispose this instance
        /// </summary>
        /// <param name="fromIDisposableDispose">call from IDisposable.Dispsose</param>
        protected override void Dispose(bool fromIDisposableDispose)
        {
            if (fromIDisposableDispose)
            {
                _movedWindow = null;

                if (_guider != null)
                {
                    _guider.Dispose();
                    _guider = null;
                }
            }
        }

        #endregion Protected section

        #region Private section
        #region Received events

        /// <summary>
        /// On host moved
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">event argument</param>
        private void OnHostMoved(object sender, EventArgs e)
        {
            if (_movedWindow != null)
            {
                GuideForm();
            }
        }

        /// <summary>
        /// On host size changed
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">event argument</param>
        private void OnHostSizeChanged(object sender, EventArgs e)
        {
            if (_movedWindow != null)
            {
                GuideForm();
            }
        }

        /// <summary>
        /// On host visibility changed
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">event argument</param>
        private void OnHostVisibleChanged(object sender, EventArgs e)
        {
            if (_host.Visible)
            {
                _guider.Initialize();
            }
        }

        #endregion Received events

        /// <summary>
        /// Show the guiders
        /// </summary>
        private void ShowMarginGuiders()
        {
            Rectangle marginBounds = _host.ScreenClientRectangle;
            if (marginBounds.IsEmpty == false)
            {
                _guider.ShowMarginsGuider(_allowedDock, marginBounds);
            }
        }

        /// <summary>
        /// Guide the form movement
        /// </summary>
        private void GuideForm()
        {
            ShowMarginGuiders();
        }

        /// <summary>
        /// Stop the form movement
        /// </summary>
        private void StopMovement()
        {
            _guider.HideCenterGuider();
            _guider.HideMarginsGuider();
            _guider.HidePreviewPanel();
        }

        /// <summary>
        /// Get container under mouse
        /// </summary>
        /// <param name="screenLocation">screen location</param>
        /// <returns>container under mouse</returns>
        private DockingContainer GetContainerUnderMouse(Point screenLocation)
        {
            Control underMouse = GetLeafDockedContainerFromPoint(screenLocation, _movedWindow);
            while (underMouse != null)
            {
                if (underMouse == _movedWindow)
                {
                    underMouse = null;
                    break;
                }

                if (underMouse as DockingContainer != null)
                {
                    break;
                }

                underMouse = underMouse.Parent;
            }

            DockingContainer containerUnderMouse = (DockingContainer)underMouse;
            if (containerUnderMouse != null)
            {
                if (containerUnderMouse.SingleChild == null)
                {
                    containerUnderMouse = null;
                }
                if (containerUnderMouse.SingleChild.AllowedDock != _allowedDock)
                {
                    containerUnderMouse = null;
                }
            }

            return containerUnderMouse;
        }

        #endregion Private section
    }
}
