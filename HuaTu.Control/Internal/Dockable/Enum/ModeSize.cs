using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuaTu.Controls.Internal.Dockable.Enum
{
    /// <summary>
    /// Size mode
    /// </summary>
    internal enum ModeSize
    {
        /// <summary>
        /// No size 
        /// </summary>
        None = 0,
        /// <summary>
        /// Resize from left edge
        /// </summary>
        Left = 1,
        /// <summary>
        /// Resize from right edge
        /// </summary>
        Right = 2,
        /// <summary>
        /// Resize from top edge
        /// </summary>
        Top = 4,
        /// <summary>
        /// Resize from bottom edge
        /// </summary>
        Bottom = 8,
        /// <summary>
        /// Move
        /// </summary>
        Move = 16,
    }
}
