using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.Command
{
    public class ReRotateCommand : ICommand
    {
        private readonly DrawObject _itemRerotated;
        private readonly PointF _oldPoint;
        private readonly PointF _newPoint;

        private ReRotateCommand()
        {
        }

        public ReRotateCommand(DrawObject itemResized, PointF old, PointF newP)
        {
            _itemRerotated = itemResized;
            _oldPoint = new PointF(old.X, old.Y);
            _newPoint = new PointF(newP.X, newP.Y);
        }

        #region 函数

        public void Execute()
        {
            _itemRerotated.RotateKnobTo(_newPoint);
        }

        public void UnExecute()
        {
            _itemRerotated.RotateKnobTo(_oldPoint);
        }

        #endregion
    }
}
