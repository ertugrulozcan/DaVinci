using Ertis.Shared.ModalWindow.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ertis.Shared.ModalWindow.Contracts
{
    interface IDialogHost
    {
        void ShowDialog(DialogBaseControl dialog);
        void HideDialog(DialogBaseControl dialog);
        FrameworkElement GetCurrentContent();
        FrameworkElement GetCurrentRegion();
    }
}
