using DrawWork;
using HuaTuDemo.Tools;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace HuaTuDemo
{
    /// <summary>
    /// Ellipse tool
    /// </summary>
    public class ToolText : ToolRectangle
    {
        #region 构造器

        public ToolText()
        {
            //Cursor = new Cursor(GetType(), "Text.cur");
            Cursor = new Cursor(Assembly.GetExecutingAssembly().GetManifestResourceStream("HuaTuDemo.Resources.Text.cur"));
            MinSize = new System.Drawing.Size(40, 20);
        }

        #endregion 构造器

        #region 函数

        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            AddNewObject(drawArea, new DrawTextObject(e.X, e.Y));
        }

        protected override void adjustForMinimumSize(DrawArea drawArea)
        {
            var objectAdded = (DrawTextObject)drawArea.GraphicsList[0];
            Rectangle rect;

            rect = objectAdded.Rect;

            if (MinSize.Width > 0)
            {
                if (objectAdded.Rect.Width < MinSize.Width)
                {
                    rect.Width = (int)(MinSize.Width * DrawObject.Zoom);
                }
            }
            if (MinSize.Height > 0)
            {
                if (objectAdded.Rect.Height < MinSize.Height)
                {
                    rect.Height = (int)(MinSize.Height * DrawObject.Zoom);
                }
            }

            objectAdded.Rect = rect;
        }

        #endregion 函数
    }
}