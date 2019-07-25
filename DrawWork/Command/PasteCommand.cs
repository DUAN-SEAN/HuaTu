using System.Collections;

namespace DrawWork.Command
{
    internal class PasteCommand : ICommand
    {
        private readonly ArrayList _graphicsList;
        private readonly ArrayList _toBePasted;

        //Disable default constructor
        private PasteCommand()
        {
        }

        public PasteCommand(ArrayList graphicsList, ArrayList toBePasted)
        {
            _graphicsList = graphicsList;
            //_toBePasted = new ArrayList();
            //_toBePasted.AddRange(toBePasted);
            _toBePasted = toBePasted;
        }

        #region ICommand Members

        public void Execute()
        {
            int n = _toBePasted.Count;

            for (int i = n - 1; i >= 0; i--)
            {
                var obj = (DrawObject)_toBePasted[i];
                obj.Move(10, 10);
                obj.Selected = true;
                _graphicsList.Insert(0, obj);
            }
        }

        public void UnExecute()
        {
            int n = _toBePasted.Count;

            for (int i = n - 1; i >= 0; i--)
            {
                _graphicsList.Remove(_toBePasted[i]);
            }
        }

        #endregion
    }
}