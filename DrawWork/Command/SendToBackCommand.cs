using System.Collections;

namespace DrawWork.Command
{
    class SendToBackCommand : ICommand
    {
        private readonly ArrayList _graphicsList;
        private readonly ArrayList _objectsBroughtForward;
        private readonly ArrayList _zOrderBackup;

        private SendToBackCommand()
        {
        }

        public SendToBackCommand(
            ArrayList graphicsList,
            ArrayList shapesToBringForward)
        {
            _objectsBroughtForward = shapesToBringForward;
            _graphicsList = graphicsList;
            _zOrderBackup = new ArrayList();
        }

        #region 函数

        public void Execute()
        {
            int i;

            var tempList = new ArrayList();
            int n = _graphicsList.Count;

            // Read source list in reverse order, add every selected item
            // to temporary list and remove it from source list
            for (i = n - 1; i >= 0; i--)
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

            // Read temporary list in reverse order and add every item
            // to the end of the source list
            n = tempList.Count;

            for (i = n - 1; i >= 0; i--)
            {
                _graphicsList.Add(tempList[i]);
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

        #endregion
    }
}