using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DrawTools;
using DrawWork;

namespace HuaTuDemo.Tools
{
    public class ToolConnect : ToolObject
    {
        #region Fields

        private DrawConnectObject _newPath;
        bool _startPathDraw = true;

        #endregion Fields
        #region 构造器

        public ToolConnect()
        {
            //Cursor = new Cursor(GetType(), "Line.cur");
            Cursor = new Cursor(Assembly.GetExecutingAssembly().GetManifestResourceStream("HuaTuDemo.Resources.Pencil.cur"));
        }

        #endregion 构造器

        #region 函数

        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ToolActionCompleted();
                return;
            }

            // Create new polygon, add it to the list
            // and keep reference to it
            if (_startPathDraw)
            {
                _newPath = new DrawConnectObject(e.X, e.Y);
                AddNewObject(drawArea, _newPath);
                _startPathDraw = false;
                IsComplete = false;
            }
            else
            {
                _newPath.AddPoint(e.Location);
            }
        }

        public override void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
        {
            drawArea.Cursor = Cursor;
            if (e.Button == MouseButtons.Left)
            {
                var point = new Point(e.X, e.Y);
                _newPath.MoveHandleTo(point, _newPath.HandleCount);
                drawArea.Refresh();
            }
        }
        public override void OnMouseUp(DrawArea drawArea, MouseEventArgs e)
        {
        }

        public override void ToolActionCompleted()
        {
            if (_newPath != null)
                _newPath.CloseFigure();
            _startPathDraw = true;
            IsComplete = true;
            _newPath = null;
        }
        #endregion 函数
    }
}
