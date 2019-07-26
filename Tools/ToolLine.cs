using DrawWork;
using HuaTuDemo.Tools;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace HuaTuDemo
{
    /// <summary>
    /// Line tool
    /// </summary>
    public class ToolLine : ToolObject
    {
        #region 构造器

        public ToolLine()
        {
            //Cursor = new Cursor(GetType(), "Line.cur");
            Cursor = new Cursor(Assembly.GetExecutingAssembly().GetManifestResourceStream("HuaTuDemo.Resources.Line.cur"));
        }

        #endregion 构造器

        #region 函数

        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            AddNewObject(drawArea, new DrawConnectLine(e.X, e.Y, e.X + 1, e.Y + 1));
            IsComplete = true;
        }

        public override void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
        {
            drawArea.Cursor = Cursor;
            if (e.Button == MouseButtons.Left)
            {
                var point = new Point(e.X, e.Y);
                drawArea.GraphicsList[0].MoveHandleTo(point, 3);
                drawArea.Refresh();
            }
        }

        #endregion 函数
    }
}