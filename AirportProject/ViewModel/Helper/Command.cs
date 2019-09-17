using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AirportProject.ViewModel.Helper
{
    class Command : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private Action<object> ExecuteAction;
        private Func<object, bool> CanExecuteFunc; 

        public Command(Action<object> action , Func<object , bool > func = null)
        {
            ExecuteAction = action;
            CanExecuteFunc = func;
        }

        public bool CanExecute(object parameter)
        {
            if (CanExecuteFunc != null) return CanExecuteFunc(parameter);
            return true; 
        }

        public void Execute(object parameter)
        {
            this.ExecuteAction(parameter);
        }
    }
}
