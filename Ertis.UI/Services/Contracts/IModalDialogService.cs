using Ertis.Shared.ModalWindow.Manager;
using Ertis.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Ertis.Shared.Services.Contracts
{
    public interface IModalDialogService
    {
        UserControl OpenDialog(ModalViewMenuItem vmi, DialogMode dialogMode);

        void OpenDialog(ModalViewMenuItem vmi, object parameter, DialogMode dialogMode);

        void CloseDialog(ModalViewMenuItem vmi);
    }
}
