using System.Collections;
using System.Drawing;

namespace DrawWork.Command
{
    using System.Collections;
    using System.Drawing;

    class MoveCommand : ICommand
    {
        #region 字段

        private readonly ArrayList _graphicListMoved;
        private PointF _deltaMoved;

        #endregion 字段

        #region 构造器

        public MoveCommand(ArrayList itemsMoved, PointF delta)
        {
            _graphicListMoved = new ArrayList();
            _deltaMoved = new PointF();

            _graphicListMoved.AddRange(itemsMoved);
            _deltaMoved = delta;
        }

        //Disable default constructor
        private MoveCommand()
        {
        }

        #endregion 构造器

        #region 函数

        public void Execute()
        {
            for (int i = 0; i < _graphicListMoved.Count; i++)
            {
                ((DrawObject)_graphicListMoved[i]).Move(_deltaMoved.X, _deltaMoved.Y);
            }
        }

        public void UnExecute()
        {
            for (int i = 0; i < _graphicListMoved.Count; i++)
            {
                ((DrawObject)_graphicListMoved[i]).Move(-1 * _deltaMoved.X, -1 * _deltaMoved.Y);
            }
        }

        #endregion 函数
    }
}