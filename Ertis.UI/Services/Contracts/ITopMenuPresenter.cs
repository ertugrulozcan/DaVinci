using Ertis.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Shared.Services.Contracts
{
    public interface ITopMenuPresenter
    {
        List<IViewMenuItem> GetTopMenuVmiList();
    }
}
