using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExpSystem.MVVM
{
    class Command : ICommand
    {
        public Command(Action action)
        {
            this.action = action;
        }

        private Action action;
        public event EventHandler CanExecuteChanged;

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
