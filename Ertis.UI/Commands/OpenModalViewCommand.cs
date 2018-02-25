using Ertis.Shared.ModalWindow.Manager;
using Ertis.Shared.Models;
using Ertis.Shared.Services.Contracts;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ertis.Shared.Commands
{
    public class OpenModalViewCommand : ICommand
    {
        public ModalViewMenuItem vmi { get; set; }

        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="viewMenuItem"></param>
        public OpenModalViewCommand(ModalViewMenuItem viewMenuItem)
        {
            vmi = viewMenuItem;
        }

        public void Execute(object parameter)
        {
            var modalDialogService = ServiceLocator.Current.GetInstance<IModalDialogService>();
            modalDialogService.OpenDialog(this.vmi, parameter, DialogMode.OkCancel);
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
    }
}
