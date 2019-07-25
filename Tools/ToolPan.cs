using HuaTuDemo.Tools;
using System.Reflection;
using System.Windows.Forms;

namespace HuaTuDemo
{
    /// <summary>
    /// Rectangle tool
    /// </summary>
    public class ToolPan : ToolObject
    {
        #region 字段

        readonly Cursor _closedHand;
        readonly Cursor _openHand;

        #endregion 字段

        #region 构造器

        public ToolPan()
        {
            _openHand = new Cursor(Assembly.GetExecutingAssembly().GetManifestResourceStream("HuaTuDemo.Resources.pan.cur"));
            _closedHand = new Cursor(Assembly.GetExecutingAssembly().GetManifestResourceStream("HuaTuDemo.Resources.pan_close.cur"));
        }

        #endregion 构造器

        #region 函数

        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            drawArea.Cursor = _closedHand;
        }

        public override void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
        {
            drawArea.Cursor = e.Button == MouseButtons.Left ? _closedHand : _openHand;
        }

        public override void OnMouseUp(DrawArea drawArea, MouseEventArgs e)
        {
            Cursor = _openHand;
        }

        #endregion 函数
    }
}