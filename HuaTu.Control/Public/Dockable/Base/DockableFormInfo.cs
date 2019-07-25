using HuaTu.Controls.Internal;
using HuaTu.Controls.Internal.Dockable;
using HuaTu.Controls.Public.Dockable.Enum;
using HuaTu.Controls.Public.UnitPagePackage.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HuaTu.Controls.Public.Dockable.Base
{
    /// <summary>
    /// 可停靠表单信息的实现
    /// </summary>
    public class DockableFormInfo : DisposablePage
    {
        #region 字段

        private Guid _identifier = Guid.Empty;
        private Form _dockableForm = null;
        private DockAllowed _allowedDock = DockAllowed.All;
        private DockStyle _hostContainerDock = DockStyle.None;
        private DockStyle _autoHideSavedDock = DockStyle.None;
        private DockStyle _dock = DockStyle.None;
        private DockableMode _dockMode = DockableMode.Outer;
        private UnitButton _button = null;
        private bool _isSelected = false;
        private bool _isAutoHideMode = false;
        private bool _showCloseButton = true;
        private bool _showContextMenuButton = true;

        #endregion 字段

        #region 构造器

        internal DockableFormInfo(Form form, DockAllowed allowedDock, Guid identifier)
        {
            if (identifier == Guid.Empty)
            {
                throw new ArgumentException("Err");
            }

            _identifier = identifier;
            _dockableForm = form;
            _allowedDock = allowedDock;
            _button = new UnitButton(form);
            _button.ExplicitDisposing += OnButtonDisposing;

            form.GotFocus += OnFormGotFocus;
        }

        #endregion 构造器

        #region 公开函数

        /// <summary>
        /// 在更改控件IsSelected属性时发生
        /// </summary>
        public event EventHandler SelectedChanged;

        /// <summary>
        /// 在更改控件IsAutoHideMode属性时发生
        /// </summary>
        public event EventHandler AutoHideModeChanged;

        /// <summary>
        /// 显示自动面板
        /// </summary>
        public event EventHandler ShowAutoPanel;


        /// <summary>
        /// 相等运算符
        /// </summary>
        public static bool operator ==(DockableFormInfo a, DockableFormInfo b)
        {
            if (((object)a) == ((object)b))
            {
                return true;
            }

            if (((object)a) == null || ((object)b) == null)
            {
                return false;
            }

            if (a._dockableForm == b._dockableForm)
            {
                if (a._identifier != b._identifier)
                {
                    throw new InvalidOperationException("Err");
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 不等式运算符
        /// </summary>
        public static bool operator !=(DockableFormInfo a, DockableFormInfo b)
        {
            return (a == b) == false;
        }


        /// <summary>
        /// 可停靠表单标识符的访问器
        /// </summary>
        public Guid Id
        {
            get
            {
                ValidateNotDisposed();

                return _identifier;
            }
        }

        /// <summary>
        /// 可停靠窗体的访问器
        /// </summary>
        public Form DockableForm
        {
            get
            {
                ValidateNotDisposed();

                return _dockableForm;
            }
        }

        /// <summary>
        /// 允许停靠模式的访问器
        /// </summary>
        public DockAllowed AllowedDock
        {
            get
            {
                ValidateNotDisposed();

                return _allowedDock;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DockStyle HostContainerDock
        {
            get
            {
                ValidateNotDisposed();

                return _hostContainerDock;
            }
            internal set
            {
                ValidateNotDisposed();

                if (_hostContainerDock != value)
                {
                    if (value == Globals.DockAutoHide)
                    {
                        _autoHideSavedDock = _hostContainerDock;
                    }

                    _hostContainerDock = value;
                }
            }
        }

        /// <summary>
        /// 窗体当前保存的自动隐藏停靠的访问器
        /// </summary>
        public DockStyle AutoHideSavedDock
        {
            get
            {
                ValidateNotDisposed();

                return _autoHideSavedDock;
            }
        }

        /// <summary>
        /// 窗体当前停靠的访问器
        /// </summary>
        public DockStyle Dock
        {
            get
            {
                ValidateNotDisposed();

                return _dock;
            }
            internal set
            {
                ValidateNotDisposed();

                _dock = value;
            }
        }

        /// <summary>
        /// 窗体当前停靠的访问器
        /// </summary>
        public DockableMode DockMode
        {
            get
            {
                ValidateNotDisposed();

                return _dockMode;
            }
            internal set
            {
                ValidateNotDisposed();

                _dockMode = value;
            }
        }

        /// <summary>
        /// 与窗体关联的按钮
        /// </summary>
        internal UnitButton Button
        {
            get
            {
                ValidateNotDisposed();

                return _button;
            }
        }


        /// <summary>
        /// 显示关闭按钮
        /// </summary>
        public bool ShowCloseButton
        {
            get { return _showCloseButton; }
            set { _showCloseButton = value; }
        }

        /// <summary>
        /// 显示上下文菜单按钮
        /// </summary>
        public bool ShowContextMenuButton
        {
            get { return _showContextMenuButton; }
            set { _showContextMenuButton = value; }
        }

        /// <summary>
        /// IsSelected标志的访问器
        /// </summary>
        public bool IsSelected
        {
            get
            {
                ValidateNotDisposed();

                return _isSelected;
            }
            set
            {
                ValidateNotDisposed();

                if (_isSelected != value)
                {
                    _isSelected = value;

                    EventHandler handler = SelectedChanged;
                    if (handler != null)
                    {
                        handler(this, EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// IsAutoHideMode标志的访问器
        /// </summary>
        public bool IsAutoHideMode
        {
            get
            {
                ValidateNotDisposed();

                return _isAutoHideMode;
            }
            internal set
            {
                ValidateNotDisposed();

                if (_isAutoHideMode != value)
                {
                    _isAutoHideMode = value;

                    EventHandler handler = AutoHideModeChanged;
                    if (handler != null)
                    {
                        handler(this, EventArgs.Empty);
                    }
                }
            }
        }


        /// <summary>
        /// 显示窗体自动面板
        /// </summary>
        public void ShowFormAutoPanel()
        {
            EventHandler handler = ShowAutoPanel;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }


        /// <summary>
        /// 检查此实例是否与obj实例相等
        /// </summary>
        public override bool Equals(object obj)
        {
            ValidateNotDisposed();

            DockableFormInfo dockable = obj as DockableFormInfo;
            if (dockable != (DockableFormInfo)null)
            {
                return this == dockable;
            }

            Form form = obj as Form;

            return DockableForm == form;
        }

        /// <summary>
        /// 获取此实例的哈希代码
        /// </summary>
        /// <returns>hash code</returns>
        public override int GetHashCode()
        {
            ValidateNotDisposed();

            return DockableForm.GetHashCode();
        }

        /// <summary>
        /// 文本
        /// </summary>
        public override string ToString()
        {
            if (DockableForm != null)
            {
                return "DFI: " + DockableForm.ToString();
            }

            return base.ToString();
        }

        #endregion 公开函数

        #region 可继承函数

        /// <summary>
        /// 释放当前实例
        /// </summary>
        protected override void Dispose(bool fromIDisposableDispose)
        {
            if (fromIDisposableDispose)
            {
                _button.ExplicitDisposing -= OnButtonDisposing;
                _button.Dispose();

                _dockableForm.GotFocus -= OnFormGotFocus;
                _dockableForm = null;
            }
        }

        #endregion 可继承函数

        #region 私有函数

        /// <summary>
        /// Occurs when the button is disposed
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">event argument</param>
        private void OnButtonDisposing(object sender, EventArgs e)
        {
            Dispose();
        }

        /// <summary>
        /// Occurs when the form got focus
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">event argument</param>
        private void OnFormGotFocus(object sender, EventArgs e)
        {
            IsSelected = true;
        }

        #endregion 私有函数
    }

}
