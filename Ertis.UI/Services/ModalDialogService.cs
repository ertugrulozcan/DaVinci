using Ertis.Shared.ModalWindow.Contracts;
using Ertis.Shared.ModalWindow.Dialogs;
using Ertis.Shared.ModalWindow.Manager;
using Ertis.Shared.Models;
using Ertis.Shared.Services.Contracts;
using Ertis.Shared.ViewModels;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Ertis.Shared.Services
{
    public class ModalDialogService : IModalDialogService
    {
        private readonly IDialogManager dialogManager;

        private Dictionary<string, IDialog> ViewNameDialogDictionary = new Dictionary<string, IDialog>();
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dialogManager"></param>
        public ModalDialogService(IDialogManager dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public UserControl OpenDialog(ModalViewMenuItem vmi, DialogMode dialogMode)
        {
            var view = (UserControl)ServiceLocator.Current.GetInstance(vmi.ViewType);
            
            IDialog dialog = this.dialogManager.CreateCustomContentDialog(view, vmi.Title, dialogMode);
            dialog.CloseBehavior = DialogCloseBehavior.ExplicitClose;

            dialog.OnDialogOpened += Dialog_OnDialogOpened;

            if (view.DataContext != null && view.DataContext is BaseViewModel)
            {
                var viewModel = view.DataContext as BaseViewModel;
                
                if (viewModel is ICustomOkCancelControl)
                {
                    var cocvm = (viewModel as ICustomOkCancelControl);

                    dialog.Cancel = () =>
                    {
                        if (cocvm.CancelClicked())
                            CloseDialog(vmi);
                    };

                    dialog.Ok = () =>
                    {
                        if (cocvm.OkClicked())
                            CloseDialog(vmi);
                    };
                }
            }
            else
            {
                dialog.Cancel = () =>
                {
                    CloseDialog(vmi);
                };
            }

            dialog.Show();

            if (!this.ViewNameDialogDictionary.ContainsKey(vmi.ViewName))
                this.ViewNameDialogDictionary.Add(vmi.ViewName, dialog);

            return view;
        }

        public void OpenDialog(ModalViewMenuItem vmi, object parameter, DialogMode dialogMode)
        {
            var view = this.OpenDialog(vmi, dialogMode);

            if (view.DataContext != null && view.DataContext is BaseViewModel)
            {
                var viewModel = view.DataContext as BaseViewModel;
                viewModel.ForceFireNavigateEvent(parameter);
            }
        }

        private void Dialog_OnDialogOpened(object sender, EventArgs e)
        {
            var dialog = sender as IDialog;



            dialog.OnDialogOpened -= Dialog_OnDialogOpened;
        }

        public void CloseDialog(ModalViewMenuItem vmi)
        {
            if (this.ViewNameDialogDictionary.ContainsKey(vmi.ViewName))
            {
                var dialog = this.ViewNameDialogDictionary[vmi.ViewName];

                if (dialog is ICustomContentDialog)
                {
                    var userControl = (((ICustomContentDialog)dialog).Content) as UserControl;

                    if (userControl is IDisposable)
                        (userControl as IDisposable).Dispose();


                    if (userControl.DataContext is IDisposable)
                        (userControl.DataContext as IDisposable).Dispose();
                }

                this.ViewNameDialogDictionary.Remove(vmi.ViewName);
                dialog.Close();

                vmi.OnCloseDialog();
            }
        }
    }
}