using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.Command
{
    using System.Collections;

    struct State
    {
        #region 字段

        public DrawObject Obj;
        public int Zorder;

        #endregion 字段
    }

    class BringToFrontCommand : ICommand
    {
        #region 字段

        private readonly ArrayList _graphicsList;
        private readonly ArrayList _objectsBroughtForward;
        private readonly ArrayList _zOrderBackup;

        #endregion 字段

        #region 构造器

        public BringToFrontCommand(
            ArrayList graphicsList,
            ArrayList shapesToBringForward)
        {
            _objectsBroughtForward = shapesToBringForward;
            _graphicsList = graphicsList;
            _zOrderBackup = new ArrayList();
        }

        private BringToFrontCommand()
        {
        }

        #endregion 构造器

        #region 函数

        public void Execute()
        {
            var tempList = new ArrayList();
            int n = _graphicsList.Count;

            for (int i = n - 1; i >= 0; i--)
            {
                if (_objectsBroughtForward.Contains(_graphicsList[i]))
                {
                    State oldState;

                    //Undo Redo -- Start
                    oldState.Obj = ((DrawObject)_graphicsList[i]);
                    oldState.Zorder = i;
                    _zOrderBackup.Add(oldState);
                    //Undo Redo -- End

                    tempList.Add(_graphicsList[i]);
                    _graphicsList.RemoveAt(i);
                }
            }

            n = tempList.Count;

            for (int i = 0; i < n; i++)
            {
                _graphicsList.Insert(0, tempList[i]);
            }
        }

        public void UnExecute()
        {
            for (int i = 0; i < _objectsBroughtForward.Count; i++)
            {
                var state = (State)_zOrderBackup[i];
                _graphicsList.Remove(state.Obj);
                _graphicsList.Insert(state.Zorder, state.Obj);
            }
        }

        #endregion 函数
    }
}
