using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.Command
{
    public class RotateCommand : ICommand
    {
        private readonly DrawObject _itemRerotated;
        private readonly PointF _oldPoint;
        private readonly PointF _newPoint;

        private RotateCommand()
        {
        }

        public RotateCommand(DrawObject itemResized, PointF old, PointF newP)
        {
            _itemRerotated = itemResized;
            _oldPoint = new PointF(old.X, old.Y);
            _newPoint = new PointF(newP.X, newP.Y);
        }

        #region 函数

        public void Execute()
        {
            var angle = ((_newPoint.X - _oldPoint.X) + (_newPoint.Y - _oldPoint.Y)) / 2;
            _itemRerotated.Rotate(angle);
        }

        public void UnExecute()
        {
            var angle = -((_newPoint.X-_oldPoint.X)+ (_newPoint.Y - _oldPoint.Y))/2;
            _itemRerotated.Rotate(angle);
        }

        #endregion
    }
}
