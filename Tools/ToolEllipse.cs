using HuaTuDemo.Tools;
using System.Reflection;
using System.Windows.Forms;
using DrawWork;
namespace HuaTuDemo
{
    public class ToolEllipse : ToolRectangle
    {
        #region 构造器

        public ToolEllipse()
        {
            //Cursor = new Cursor(GetType(), "Ellipse.cur");
            Cursor = new Cursor(Assembly.GetExecutingAssembly().GetManifestResourceStream("HuaTuDemo.Resources.Ellipse.cur"));
        }

        #endregion 构造器

        #region 函数

        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            AddNewObject(drawArea,new DrawCircleObject(e.X,e.Y,1,1));
           // AddNewObject(drawArea, new DrawEllipseObject(e.X, e.Y, 1, 1));
        }

        #endregion 函数
    }
}