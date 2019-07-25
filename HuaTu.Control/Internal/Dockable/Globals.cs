using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HuaTu.Controls.Internal.Dockable
{
    internal class Globals
    {
        /// <summary>
        /// 未设置停靠样式
        /// </summary>
        public const DockStyle DockNotSet = (DockStyle)Int32.MinValue;
        /// <summary>
        /// 停靠样式自动隐藏
        /// </summary>
        public const DockStyle DockAutoHide = (DockStyle)Int32.MaxValue;
    }
}
