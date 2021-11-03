using KvantText.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KvantText.Models
{
    abstract class BaseDelegate<T,N> : IDelegateCommand
    {
        readonly Action<T,N> execute;
        readonly Func<object, bool> canExecute;
        public event EventHandler CanExecuteChanged;

        public BaseDelegate(Action<T,N> execute, Func<object, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public BaseDelegate(Action<T,N> execute)
        {
            this.execute = execute;
            canExecute = AlwaysCanExecute;
        }
        public bool CanExecute(object parameter)
        {
            return canExecute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
        private bool AlwaysCanExecute(object param)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            execute((T)parameter, default(N));
        }
    }

    abstract class BaseDelegate<T> : IDelegateCommand
    {
        readonly Action<T> execute;
        readonly Func<object, bool> canExecute;
        public event EventHandler CanExecuteChanged;

        public BaseDelegate(Action<T> execute, Func<object, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public BaseDelegate(Action<T> execute)
        {
            this.execute = execute;
            canExecute = AlwaysCanExecute;
        }
        public bool CanExecute(object parameter)
        {
            return canExecute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
        private bool AlwaysCanExecute(object param)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            execute((T)parameter);
        }
    }
}
