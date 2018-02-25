using Ertis.Shared.ModalWindow.Contracts;
using Ertis.Shared.ModalWindow.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Ertis.Shared.ModalWindow.Dialogs
{
    class CustomContentDialog : DialogBase, ICustomContentDialog
    {
        public CustomContentDialog(IDialogHost dialogHost, DialogMode dialogMode, object content, Dispatcher dispatcher) : base(dialogHost, dialogMode, dispatcher)
        {
            SetContent(content);
        }
    }
}
