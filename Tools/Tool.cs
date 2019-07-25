using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HuaTuDemo.Tools
{
    public abstract class Tool
    {
        #region 字段

        /// <summary>
        /// 如果为false，则该工具尚未完成
        /// </summary>
        public Boolean IsComplete;

        #endregion 字段

        #region 函数

        /// <summary>
        /// 按下左键
        /// </summary>
        public virtual void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
        }

        /// <summary>
        /// 移动鼠标，按下鼠标左键或不按下按钮
        /// </summary>
        public virtual void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
        {
        }

        /// <summary>
        /// 释放鼠标左键
        /// </summary>
        public virtual void OnMouseUp(DrawArea drawArea, MouseEventArgs e)
        {
        }

        public virtual void ToolActionCompleted()
        {
        }

        #endregion 函数
    }
}
