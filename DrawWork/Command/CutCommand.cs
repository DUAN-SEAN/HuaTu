using System.Collections;

namespace DrawWork.Command
{
    internal class CutCommand : ICommand
    {
        #region 字段

        private readonly ArrayList _graphicsList;
        private readonly ArrayList _itemsToBeCut;

        #endregion 字段

        #region 构造器

        public CutCommand(ArrayList graphicsList, ArrayList inMemory)
        {
            _graphicsList = graphicsList;
            _itemsToBeCut = new ArrayList();
            _itemsToBeCut.AddRange(inMemory);
        }

        //Disable default constructor
        private CutCommand()
        {
        }

        #endregion 构造器

        #region 函数

        public void Execute()
        {
            for (int i = _graphicsList.Count - 1; i >= 0; i--)
            {
                if (_itemsToBeCut.Contains(_graphicsList[i]))
                {
                    _graphicsList.RemoveAt(i);
                }
            }
        }

        public void UnExecute()
        {
            for (int i = 0; i < _itemsToBeCut.Count; i++)
            {
                _graphicsList.Add(_itemsToBeCut[i]);
            }
            _itemsToBeCut.Clear();
        }

        #endregion 函数
    }
}