using Ertis.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Shared.Interfaces
{
    public interface IShell
    {
        BaseViewModel LeftSidePanelViewModel { get; }
    }
}
