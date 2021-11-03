using KvantText.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KvantText.Models
{
    class DelegateEvent : EventArgs,IDelegateCommand
    {
        private Action<object> execute;
        readonly Func<object, bool> canExecute;
        public event EventHandler CanExecuteChanged;
        public DelegateEvent(Action<object> execute, Func<object, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }
        public DelegateEvent(Action<object> execute)
        {
            this.execute = execute;
            canExecute = AlwaysCanExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
        private bool AlwaysCanExecute(object param)
        {
            return true;
        }
    }
}
