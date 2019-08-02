using DrawWork;
using HuaTuDemo.Tools;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace HuaTuDemo
{
    /// <summary>
    /// Polygon tool
    /// </summary>
    public class ToolPath : ToolObject
    {
        #region 字段

        private DrawPathObject _newPath;
        bool _startPathDraw = true;

        #endregion 字段

        #region 构造器

        public ToolPath()
        {
            //Cursor = new Cursor(GetType(), "Pencil.cur");
            Cursor = new Cursor(Assembly.GetExecutingAssembly().GetManifestResourceStream("HuaTuDemo.Resources.Pencil.cur"));
        }

        #endregion 构造器

        #region Methods

        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Right)
            //{
            //    ToolActionCompleted();
            //    return;
            //}

            //if (_startPathDraw)
            //{
            //    _newPath = new DrawPathObject(e.X, e.Y);
            //    AddNewObject(drawArea, _newPath);
            //    _startPathDraw = false;
            //    IsComplete = false;
            //}
            //else
            //{
            //    _newPath.AddPoint(e.Location);
            //}
            AddNewObject(drawArea, new WireConnectLineDrawObject(e.X, e.Y, e.X + 1, e.Y + 1));
            IsComplete = true;
        }

        public override void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
        {
            //drawArea.Cursor = Cursor;
            //if (e.Button == MouseButtons.Left)
            //{
            //    var point = new Point(e.X, e.Y);
            //    _newPath.MoveHandleTo(point, _newPath.HandleCount);
            //    drawArea.Refresh();
            //}
            drawArea.Cursor = Cursor;
            if (e.Button == MouseButtons.Left)
            {
                var point = new Point(e.X, e.Y);
                drawArea.GraphicsList[0].MoveHandleTo(point, 3);
                drawArea.Refresh();
            }
        }

        public override void OnMouseUp(DrawArea drawArea, MouseEventArgs e)
        {
        }

        public override void ToolActionCompleted()
        {
            //if (_newPath != null)
            //    _newPath.CloseFigure();
            //_startPathDraw = true;
            //IsComplete = true;
            //_newPath = null;
        }

        #endregion 函数
    }
}