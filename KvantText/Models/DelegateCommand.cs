using KvantText.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KvantText.Models
{
    class DelegateCommand : BaseDelegate<object>
    {
        public DelegateCommand(Action<object> execute) : base(execute)
        {
        }
        public DelegateCommand(Action<object> execute, Func<object, bool> canExecute) : base(execute, canExecute)
        {
        }
    }
}
