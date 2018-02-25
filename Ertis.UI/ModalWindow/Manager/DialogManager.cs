using Ertis.Shared.ModalWindow.Contracts;
using Ertis.Shared.ModalWindow.Dialogs;
using Ertis.Shared.ModalWindow.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Ertis.Shared.ModalWindow.Manager
{
    public class DialogManager : IDialogManager
    {
        public DialogManager(ContentControl parent, ContentControl layer, ContentControl region, Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            _dialogHost = new DialogLayeringHelper(parent, layer, region);
        }

        private readonly Dispatcher _dispatcher;
        private readonly IDialogHost _dialogHost;

        #region Implementation of IDialogManager

        public IMessageDialog CreateMessageDialog(string message, DialogMode dialogMode)
        {
            IMessageDialog dialog = null;
            dialog = new MessageDialog(_dialogHost, dialogMode, message, _dispatcher);

            return dialog;
        }

        public IMessageDialog CreateMessageDialog(string message, string caption, DialogMode dialogMode)
        {
            IMessageDialog dialog = null;
            dialog = new MessageDialog(_dialogHost, dialogMode, message, _dispatcher)
            {
                Caption = caption
            };

            return dialog;
        }

        public IProgressDialog CreateProgressDialog(DialogMode dialogMode)
        {
            IProgressDialog dialog = null;
            InvokeInUIThread(() =>
            {
                dialog = WaitProgressDialog.CreateProgressDialog(_dialogHost, dialogMode, _dispatcher);
                dialog.CloseWhenWorkerFinished = true;
            });
            return dialog;
        }

        public IProgressDialog CreateProgressDialog(string message, DialogMode dialogMode)
        {
            IProgressDialog dialog = null;
            InvokeInUIThread(() =>
            {
                dialog = WaitProgressDialog.CreateProgressDialog(_dialogHost, dialogMode, _dispatcher);
                dialog.CloseWhenWorkerFinished = true;
                dialog.Message = message;
            });
            return dialog;
        }

        public IProgressDialog CreateProgressDialog(string message, string readyMessage, DialogMode dialogMode)
        {
            IProgressDialog dialog = null;
            InvokeInUIThread(() =>
            {
                dialog = WaitProgressDialog.CreateProgressDialog(_dialogHost, dialogMode, _dispatcher);
                dialog.CloseWhenWorkerFinished = false;
                dialog.ReadyMessage = readyMessage;
                dialog.Message = message;
            });
            return dialog;
        }

        public IWaitDialog CreateWaitDialog(DialogMode dialogMode)
        {
            IWaitDialog dialog = null;
            InvokeInUIThread(() =>
            {
                dialog = WaitProgressDialog.CreateWaitDialog(_dialogHost, dialogMode, _dispatcher);
                dialog.CloseWhenWorkerFinished = true;
            });
            return dialog;
        }

        public IWaitDialog CreateWaitDialog(string message, DialogMode dialogMode)
        {
            IWaitDialog dialog = null;
            InvokeInUIThread(() =>
            {
                dialog = WaitProgressDialog.CreateWaitDialog(_dialogHost, dialogMode, _dispatcher);
                dialog.CloseWhenWorkerFinished = true;
                dialog.Message = message;
            });
            return dialog;
        }

        public IWaitDialog CreateWaitDialog(string message, string readyMessage, DialogMode dialogMode)
        {
            IWaitDialog dialog = null;
            InvokeInUIThread(() =>
            {
                dialog = WaitProgressDialog.CreateWaitDialog(_dialogHost, dialogMode, _dispatcher);
                dialog.CloseWhenWorkerFinished = false;
                dialog.Message = message;
                dialog.ReadyMessage = readyMessage;
            });
            return dialog;
        }

        public ICustomContentDialog CreateCustomContentDialog(object content, DialogMode dialogMode)
        {
            ICustomContentDialog dialog = null;
            InvokeInUIThread(() =>
            {
                dialog = new CustomContentDialog(_dialogHost, dialogMode, content, _dispatcher);
            });
            return dialog;
        }

        public ICustomContentDialog CreateCustomContentDialog(object content, string caption, DialogMode dialogMode)
        {
            ICustomContentDialog dialog = null;
            InvokeInUIThread(() =>
            {
                dialog = new CustomContentDialog(_dialogHost, dialogMode, content, _dispatcher)
                {
                    Caption = caption
                };
            });

            return dialog;
        }

        #endregion

        private void InvokeInUIThread(Action del)
        {
            _dispatcher.Invoke(del);
        }
    }
}
