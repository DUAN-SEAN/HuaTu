using DrawWork;
using HuaTuDemo.Tools;
using System.Windows.Forms;

namespace HuaTuDemo
{
    /// <summary>
    /// Ellipse tool
    /// </summary>
    public class ToolImage : ToolRectangle
    {
        public ToolImage()
        {
            //Cursor = new Cursor(GetType(), "Bitmap.cur");
            Cursor = new Cursor(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("HuaTuDemo.Resources.Text.cur"));
        }

        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            AddNewObject(drawArea, new DrawImageObject(e.X, e.Y));
        }

    }

}