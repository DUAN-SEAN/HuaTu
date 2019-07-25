using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuaTu.Controls.Public.Dockable.Enum
{
    /// <summary>
    /// 停靠模式
    /// </summary>

    public enum DockableMode
    {
        /// <summary>
        /// 未指定
        /// </summary>
        None = 0,
        /// <summary>
        /// 外部船坞（主机外部边缘）
        /// </summary>
        Outer = 1,
        /// <summary>
        /// 内部船坞（主机可用区域内）
        /// </summary>
        Inner = 2,

    }
}
