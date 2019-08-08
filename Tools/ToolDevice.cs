using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DrawWork;
using DrawWork.Symbol;

namespace HuaTuDemo.Tools
{
    public class ToolDevice:ToolObject
    {
        private string currentDeviceId = default;
        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            currentDeviceId = drawArea.DeviceId;//获取当前的设备Id

            //TODO:根据不同的设备Id找到对用的设备信息
            SymbolUnit._Dic.TryGetValue(currentDeviceId, out SymbolUnit value);
            var id = DateTime.Now.Ticks.ToString();
            DeviceDrawObjectBase deviceDrawObjectBase = SVGFactory.CreateDeviceDrawObjectBase((float) e.X, (float) e.Y,
                100, 100, currentDeviceId, id);

            drawArea.GraphicsList.Add(deviceDrawObjectBase);

            
            base.OnMouseDown(drawArea, e);
        }

        public override void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
        {
            base.OnMouseMove(drawArea, e);
        }

        public override void OnMouseUp(DrawArea drawArea, MouseEventArgs e)
        {
            base.OnMouseUp(drawArea, e);


            //drawArea.Capture = false;
            //drawArea.Refresh();

            //IsComplete = true;

        }
    }
}
