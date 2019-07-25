using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuaTu.Controls.Internal
{
    public abstract class DisposablePage : IDisposable
    {
        #region 字段

        private bool _isDisposing = false;
        private bool _isDisposed = false;

        #endregion 字段

        #region 构造器

        protected DisposablePage()
        {
        }

        ~DisposablePage()
        {
            DisposeInstance(false);
        }

        #endregion 构造器

        #region 公开

        /// <summary>
        /// 通过调用IDisposable.Dispose为当前实例启动Disposing时引发的事件
        /// </summary>
        public event EventHandler ExplicitDisposing;

        /// <summary>
        /// GC为当前实例启动释放时引发的事件
        /// </summary>
        public event EventHandler GCDisposing;

        /// <summary>
        /// 释放当前实例后引发的事件
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        /// 指示当前实例已释放的标志
        /// </summary>
        public bool IsDisposed
        {
            get { return _isDisposed; }
            private set { _isDisposed = value; }
        }

        /// <summary>
        /// Dispose方法实现
        /// </summary>
        public void Dispose()
        {
            DisposeInstance(true);
            GC.SuppressFinalize(this);
        }

        #endregion 公开

        #region 可继承

        /// <summary>
        /// Dispose方法实现 带参数
        /// </summary>
        protected abstract void Dispose(bool fromIDisposableDispose);

        /// <summary>
        /// 验证当前实例是否Disposed
        /// </summary>
        protected void ValidateNotDisposed()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        #endregion 可继承

        #region 私有的

        /// <summary>
        /// Dispose当前实例
        /// </summary>
        private void DisposeInstance(bool fromIDisposableDispose)
        {
            if (_isDisposing)
            {
                return;
            }

            if (IsDisposed == false)
            {
                _isDisposing = true;

                EventHandler disposingHandler = ExplicitDisposing;
                if (fromIDisposableDispose == false)
                {
                    disposingHandler = GCDisposing;
                }

                try
                {
                    if (disposingHandler != null)
                    {
                        disposingHandler(this, EventArgs.Empty);
                    }

                    Dispose(fromIDisposableDispose);
                }
                finally
                {
                    IsDisposed = true;
                    _isDisposing = false;

                    EventHandler disposedHandler = Disposed;
                    if (disposedHandler != null)
                    {
                        disposedHandler(this, EventArgs.Empty);
                    }
                }
            }
        }

        #endregion 私有的
    }
}
