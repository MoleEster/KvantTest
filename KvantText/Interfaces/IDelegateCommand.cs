using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KvantText.Interfaces
{
    interface IDelegateCommand:ICommand
    {
        void RaiseCanExecuteChanged();
    }
}
