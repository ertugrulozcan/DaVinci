using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ertis.Shared.Models
{
    public interface IViewMenuItem
    {
        ICommand Command { get; }

        void Navigate();

        void Navigate<T>(T parameters);

        List<IViewMenuItem> Children { get; }
    }
}
