using System.Collections;

namespace DrawWork.Command
{
    internal class DeleteCommand : ICommand
    {
        #region 字段

        private readonly ArrayList _graphicsList;
        private readonly ArrayList _graphicsListDeleted;

        #endregion 字段

        #region 构造器

        public DeleteCommand(ArrayList graphicsList)
        {
            _graphicsList = graphicsList;
            _graphicsListDeleted = new ArrayList();
        }

        private DeleteCommand()
        {
        }

        #endregion 构造器

        #region 函数

        public void Execute()
        {
            int n = _graphicsList.Count;

            for (int i = n - 1; i >= 0; i--)
            {
                if (((DrawObject)_graphicsList[i]).Selected)
                {
                    State obj;
                    obj.Obj = (DrawObject)_graphicsList[i];
                    obj.Zorder = i;
                    _graphicsListDeleted.Add(obj);
                    _graphicsList.RemoveAt(i);
                }
            }
        }

        public void UnExecute()
        {
            for (int i = 0; i < _graphicsListDeleted.Count; i++)
            {
                var obj = (State)_graphicsListDeleted[i];
                _graphicsList.Insert(obj.Zorder, obj.Obj);
            }
        }

        #endregion 函数
    }
}