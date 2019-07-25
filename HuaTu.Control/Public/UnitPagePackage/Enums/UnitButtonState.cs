using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuaTu.Controls.Public.UnitPagePackage.Enums
{
    /// <summary>
    ///按钮状态 
    /// </summary>
    public enum UnitButtonState
    {
        /// <summary>
        /// 常规状态 
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 按钮在鼠标下
        /// </summary>
        UnderMouseCursor = 1,
        /// <summary>
        /// 按钮获得焦点
        /// </summary>
        Focus = 2,
        /// <summary>
        /// 按钮被按下
        /// </summary>
        Pressed = 6,
        /// <summary>
        /// 按钮不可用
        /// </summary>
        Disabled = 8,
    }
}
