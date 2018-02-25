using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Shared.ModalWindow.Contracts
{
    public interface ICustomContentDialog : IDialog
    {
        object Content { get; }
    }
}
