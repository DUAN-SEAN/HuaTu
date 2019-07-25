using System.Drawing;

namespace DrawWork.Command
{
    class ResizeCommand : ICommand
    {

        private readonly DrawObject _itemResized;
        private readonly PointF _oldPoint;
        private readonly PointF _newPoint;
        private readonly int _handle;

        private ResizeCommand()
        {
        }

        public ResizeCommand(DrawObject itemResized, PointF old, PointF newP, int handle)
        {
            _itemResized = itemResized;
            _oldPoint = new PointF(old.X, old.Y);
            _newPoint = new PointF(newP.X, newP.Y);
            _handle = handle;
        }

        #region 函数

        public void Execute()
        {
            _itemResized.MoveHandleTo(_newPoint, _handle);
        }

        public void UnExecute()
        {
            _itemResized.MoveHandleTo(_oldPoint, _handle);
        }

        #endregion
    }
}