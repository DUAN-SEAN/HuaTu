using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.Command
{
    public class UndoRedo
    {
        private readonly Stack<ICommand> _undoCommands = new Stack<ICommand>();
        private readonly Stack<ICommand> _redoCommands = new Stack<ICommand>();

        public void Undo()
        {
            if (_undoCommands.Count > 0)
            {
                ICommand command = _undoCommands.Pop();
                command.UnExecute();
                _redoCommands.Push(command);
            }
        }

        public void Redo()
        {
            if (_redoCommands.Count > 0)
            {
                ICommand command = _redoCommands.Pop();
                command.Execute();
                _undoCommands.Push(command);
            }
        }

        public void AddCommand(ICommand cmd)
        {
            _undoCommands.Push(cmd);
        }
    }
}
