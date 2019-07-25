using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuaTu.Controls.Public.Dockable.Enum
{
    /// <summary>
    /// 枚举如何在DockContainer内停靠控件  <see cref="DockContainer">DockContainer</see>
    /// </summary>

    public enum DockAllowed
    {
        /// <summary>
        /// 控件的允许停靠未知
        /// </summary>
        Unknown = -1,
        /// <summary>
        /// 无法停靠控件。
        /// </summary>
        None = 0,
        /// <summary>
        /// 控件可以停靠在左侧
        /// </summary>
        Left = 2,
        /// <summary>
        /// 控件可以停靠在顶部
        /// </summary>
        Top = 4,
        /// <summary>
        /// 控件可以停靠在右侧
        /// </summary>
        Right = 8,
        /// <summary>
        /// 控件可以停靠在底端
        /// </summary>
        Bottom = 16,
        /// <summary>
        /// 控件可以填充容器
        /// </summary>
        Fill = 32,
        /// <summary>
        /// 控件可以水平停靠，在 <see cref="Left"/> 或者 <see cref="Right"/>
        /// </summary>
        Horizontally = Left | Right,
        /// <summary>
        /// 控件可以垂直停靠, 在 <see cref="Top"/> 或者 <see cref="Bottom"/>
        /// </summary>
        Vertically = Top | Bottom,
        /// <summary>
        /// 控件可以停靠在容器的侧面。
        /// 可以在 <see cref="Left"/>, <see cref="Right"/>, <see cref="Top"/>, <see cref="Bottom"/>
        /// </summary>
        Sides = Horizontally | Vertically,
        /// <summary>
        /// 控件可以停靠在所有 <see cref="Sides"/>, 能 <see cref="Fill"/> 容器，并且能 <see cref="None">浮动</see>
        /// </summary>
        All = Sides | Fill | None

    }
}
