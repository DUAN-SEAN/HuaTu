using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuaTu.Controls.Public.UnitPagePackage.Data
{
    using HuaTu.Controls.Public.UnitPagePackage.Drawer;
    using HuaTu.Controls.Internal;
    using System.Drawing;
    using System.Windows.Forms;

    public class UnitButton : DisposablePage
    {
        #region 字段

        private string _text = string.Empty;
        private Rectangle _bounds = new Rectangle();
        private Control _tabPage = null;

        #endregion 字段

        #region 构造器

        public UnitButton(Control page)
        {
            _tabPage = page;
            Text = page.Text;
            _tabPage.TextChanged += OnPageTextChanged;
            _tabPage.Disposed += OnTabPageDisposed;
        }

        #endregion 构造器

        #region 公开函数

        /// <summary>
        /// 更改文本时引发的事件
        /// </summary>
        public event EventHandler TextChanged;

        /// <summary>
        /// 设置按钮边界
        /// </summary>
        public void SetBounds(Rectangle bounds)
        {
            ValidateNotDisposed();

            if (bounds.Width > 80)
            {
                bounds.Width = 80;
            }

            _bounds = bounds;
        }

        /// <summary>
        /// 检查按钮是否包含给定点
        /// </summary>
        public bool Contains(Point point)
        {
            ValidateNotDisposed();

            return _bounds.Contains(point);
        }

        /// <summary>
        /// 选项卡按钮文本的访问器
        /// </summary>
        public string Text
        {
            get
            {
                ValidateNotDisposed();

                return _text;
            }
            set
            {
                ValidateNotDisposed();

                if (_text != value)
                {
                    _text = value;

                    if (TextChanged != null)
                    {
                        TextChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// 左侧位置的存取器
        /// </summary>
        public int Left
        {
            get
            {
                ValidateNotDisposed();

                return _bounds.Left;
            }
        }

        /// <summary>
        /// 顶部位置的存取器
        /// </summary>
        public int Top
        {
            get
            {
                ValidateNotDisposed();

                return _bounds.Top;
            }
        }

        /// <summary>
        /// 宽度访问器
        /// </summary>
        public int Width
        {
            get
            {
                ValidateNotDisposed();

                return _bounds.Width;
            }
        }

        /// <summary>
        /// 高度访问器
        /// </summary>
        public int Height
        {
            get
            {
                ValidateNotDisposed();

                return _bounds.Height;
            }
        }

        /// <summary>
        /// 与按钮关联的页面
        /// </summary>
        public Control Page
        {
            get
            {
                ValidateNotDisposed();

                return _tabPage;
            }
        }

        /// <summary>
        /// 与按钮关联的图标
        /// </summary>
        public Icon PageIcon
        {
            get
            {
                ValidateNotDisposed();

                Icon icon = null;
                Form form = Page as Form;
                if (form != null)
                {
                    icon = form.Icon;
                }

                return icon;
            }
        }

        /// <summary>
        /// 绘制按钮
        /// </summary>
        public void Draw(UnitButtonDrawer renderer, bool selected, Font font, Graphics graphics)
        {
            ValidateNotDisposed();

            renderer.Draw(_bounds, Text, selected, font, PageIcon, graphics);
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
                _tabPage.TextChanged -= OnPageTextChanged;
                _tabPage.Disposed -= OnTabPageDisposed;

                _tabPage = null;

                _bounds = Rectangle.Empty;

                _text = null;
            }
        }

        #endregion 可继承函数

        #region 私有函数

        /// <summary>
        /// 页面上的文本已更改
        /// </summary>
        private void OnPageTextChanged(object sender, EventArgs e)
        {
            Text = _tabPage.Text;
        }

        /// <summary>
        /// 在释放页时发生
        /// </summary>
        private void OnTabPageDisposed(object sender, EventArgs e)
        {
            Dispose();
        }

        #endregion 私有函数
    }
}
