using System;
using System.Windows.Input;

namespace ExpSystem.MVVM
{
    class Command : ICommand
    {
        private Action action;
        public event EventHandler CanExecuteChanged;

        public Command(Action action)
        {
            this.action = action;
        }

        public bool CanExecute(object parameter)
        {
            return action != null;
        }

        public void Execute(object parameter)
        {
            action.Invoke();
        }
    }
}
