using DrawWork;
using HuaTuDemo.Tools;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace HuaTuDemo
{

    public abstract class ToolObject : Tool
    {
        #region 属性

        /// <summary>
        /// 工具光标。
        /// </summary>
        protected Cursor Cursor { get; set; }

        /// <summary>
        /// 对象的最小大小
        /// </summary>
        protected Size MinSize { get; set; }

        #endregion 属性

        #region 函数

        /// <summary>
        /// 释放鼠标左键。
        /// 创建新对象并调整其大小。
        /// </summary>
        public override void OnMouseUp(DrawArea drawArea, MouseEventArgs e)
        {
            if (drawArea.GraphicsList[0] != null)
                drawArea.GraphicsList[0].Normalize();

            adjustForMinimumSize(drawArea);

            drawArea.Capture = false;
            IsComplete = true;
            drawArea.Refresh();
        }

        /// <summary>
        /// 将新对象添加到绘图区域。
        /// 当用户左键单击绘制区域时调用函数，
        /// 并且其中一个toolObject派生工具处于活动状态。
        /// </summary>
        [CLSCompliant(false)]
        protected void AddNewObject(DrawArea drawArea, DrawObject o)
        {
            drawArea.GraphicsList.UnselectAll();

            o.Selected = true;
            drawArea.GraphicsList.Add(o);

            drawArea.Capture = true;
            drawArea.Refresh();

            drawArea.SetDirty();
        }

        protected virtual void adjustForMinimumSize(DrawArea drawArea)
        {
        }

        #endregion 函数
    }

}