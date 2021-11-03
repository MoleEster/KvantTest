using KvantText.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KvantText.Models
{
    class DelegateMouseCommand : BaseDelegate<object, MouseButtonEventArgs>
    {
        public DelegateMouseCommand(Action<object, MouseButtonEventArgs> execute) : base(execute)
        {
        }

        public DelegateMouseCommand(Action<object, MouseButtonEventArgs> execute, Func<object, bool> canExecute) : base(execute, canExecute)
        {
        }
    }
}
