using DrawWork;
using HuaTuDemo.Tools;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace HuaTuDemo
{
    public class ToolRectangle : ToolObject
    {

        public ToolRectangle()
        {
            Cursor = new Cursor(Assembly.GetExecutingAssembly().GetManifestResourceStream(@"HuaTuDemo.Resources.Rectangle.cur"));
        }

        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            AddNewObject(drawArea, new DrawRectangleObject(e.X, e.Y, 1, 1));
            //AddNewObject(drawArea, new SingleDisConnectorDrawObject(e.X, e.Y, e.X + 1, e.Y + 1,1));

        }

        public override void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
        {
            drawArea.Cursor = Cursor;

            if (e.Button == MouseButtons.Left && drawArea.GraphicsList.Count > 0)
            {
                var point = new Point(e.X, e.Y);
                drawArea.GraphicsList[0].MoveHandleTo(point, 5);
                drawArea.Refresh();
            }
        }
    }
}