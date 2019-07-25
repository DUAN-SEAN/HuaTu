using System.Collections;

namespace DrawWork.Command
{
    internal class CreateCommand : ICommand
    {
        #region 字段

        private readonly ArrayList _graphicsList;
        private readonly DrawObject _shape;

        #endregion 字段

        #region 构造器

        public CreateCommand(DrawObject shape, ArrayList graphicsList)
        {
            _shape = shape;
            _graphicsList = graphicsList;
        }

        //Disable default constructor
        private CreateCommand()
        {
        }

        #endregion 构造器

        #region 函数

        public void Execute()
        {
            _graphicsList.Insert(0, _shape);
        }

        public void UnExecute()
        {
            _graphicsList.Remove(_shape);
        }

        #endregion 函数
    }
}