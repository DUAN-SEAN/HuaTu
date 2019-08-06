using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HuaTuDemo.Tools
{
    public class ToolDevice:ToolObject
    {
        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {

            base.OnMouseDown(drawArea, e);
        }

        public override void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
        {
            base.OnMouseMove(drawArea, e);
        }

        public override void OnMouseUp(DrawArea drawArea, MouseEventArgs e)
        {
            base.OnMouseUp(drawArea, e);
        }
    }
}
