using Ertis.Shared.Models;
using Ertis.Shared.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Shared.Routing.Contracts
{
    public interface IGuiRoutableModule
    {
        IViewMenuItem ViewMenuItem { get; }
    }
}
