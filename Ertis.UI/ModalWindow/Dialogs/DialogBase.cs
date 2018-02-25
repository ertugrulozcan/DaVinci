using Ertis.Shared.ModalWindow.Contracts;
using Ertis.Shared.ModalWindow.Manager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Ertis.Shared.ModalWindow.Views;
using System.Windows;

namespace Ertis.Shared.ModalWindow.Dialogs
{
    abstract class DialogBase : IDialog, INotifyPropertyChanged
    {
        private bool isCloseButtonVisible;

        public event EventHandler OnDialogOpened;
        public event EventHandler OnDialogClosed;

        protected DialogBase(IDialogHost dialogHost, DialogMode dialogMode, Dispatcher dispatcher, bool isCloseButtonVisible = true)
        {
            _dialogHost = dialogHost;
            _dispatcher = dispatcher;
            Mode = dialogMode;
            CloseBehavior = DialogCloseBehavior.AutoCloseOnButtonClick;
            this.isCloseButtonVisible = isCloseButtonVisible;

            OkText = Localization.LocalizationUtility.Convert("Ok");
            CancelText = Localization.LocalizationUtility.Convert("Cancel");
            YesText = Localization.LocalizationUtility.Convert("Yes");
            NoText = Localization.LocalizationUtility.Convert("No");

            switch (dialogMode)
            {
                case DialogMode.None:
                    break;
                case DialogMode.Ok:
                    CanOk = true;
                    break;
                case DialogMode.Cancel:
                    CanCancel = true;
                    break;
                case DialogMode.OkCancel:
                    CanOk = true;
                    CanCancel = true;
                    break;
                case DialogMode.YesNo:
                    CanYes = true;
                    CanNo = true;
                    break;
                case DialogMode.YesNoCancel:
                    CanYes = true;
                    CanNo = true;
                    CanCancel = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("dialogMode");
            }
        }

        private readonly IDialogHost _dialogHost;
        private readonly Dispatcher _dispatcher;
        private object _content;

        public object Content { get { return _content; } set { _content = value; } }

        protected DialogBaseControl DialogBaseControl { get; private set; }

        protected void SetContent(object content)
        {
            Content = content;
        }

        protected void InvokeUICall(Action del)
        {
            _dispatcher.Invoke(del, DispatcherPriority.DataBind);
        }

        #region Implementation of IDialog

        public DialogMode Mode { get; private set; }
        public DialogResultState Result { get; set; }
        public DialogCloseBehavior CloseBehavior { get; set; }

        public Action Ok { get; set; }
        public Action Cancel { get; set; }
        public Action Yes { get; set; }
        public Action No { get; set; }

        private bool _canOk;
        public bool CanOk
        {
            get { return _canOk; }
            set
            {
                _canOk = value;
                OnPropertyChanged("CanOk");
            }
        }

        private bool _canCancel;
        public bool CanCancel
        {
            get { return _canCancel; }
            set
            {
                _canCancel = value;
                OnPropertyChanged("CanCancel");
            }
        }

        private bool _canYes;
        public bool CanYes
        {
            get { return _canYes; }
            set
            {
                _canYes = value;
                OnPropertyChanged("CanYes");
            }
        }

        private bool _canNo;
        public bool CanNo
        {
            get { return _canNo; }
            set
            {
                _canNo = value;
                OnPropertyChanged("CanNo");
            }
        }

        public string OkText { get; set; }
        public string CancelText { get; set; }
        public string YesText { get; set; }
        public string NoText { get; set; }

        public string Caption { get; set; }

        private VerticalAlignment? _verticalDialogAlignment;
        public VerticalAlignment VerticalDialogAlignment
        {
            set
            {
                if (DialogBaseControl == null)
                    _verticalDialogAlignment = value;
                else
                    DialogBaseControl.VerticalDialogAlignment = value;
            }
        }

        private HorizontalAlignment? _horizontalDialogAlignment;
        public HorizontalAlignment HorizontalDialogAlignment
        {
            set
            {
                if (DialogBaseControl == null)
                    _horizontalDialogAlignment = value;
                else
                    DialogBaseControl.HorizontalDialogAlignment = value;
            }
        }

        public void Show()
        {
            if (DialogBaseControl != null)
                throw new Exception("The dialog can only be shown once.");

            InvokeUICall(() =>
            {
                var originalContent = _dialogHost.GetCurrentContent();
                var originalRegion = _dialogHost.GetCurrentRegion();
                DialogBaseControl = new DialogBaseControl(originalContent, this, this.isCloseButtonVisible);
                originalRegion.Visibility = Visibility.Collapsed;
                DialogBaseControl.SetCustomContent(_content);

                if (_verticalDialogAlignment.HasValue)
                    DialogBaseControl.VerticalDialogAlignment = _verticalDialogAlignment.Value;
                if (_horizontalDialogAlignment.HasValue)
                    DialogBaseControl.HorizontalDialogAlignment = _horizontalDialogAlignment.Value;

                try
                {
                    _dialogHost.ShowDialog(DialogBaseControl);
                    if (this.OnDialogOpened != null)
                        this.OnDialogOpened(this, new EventArgs());
                }
                catch (Exception ex)
                {

                }
            });
        }

        public void Close()
        {
            // Dialog wird angezeigt?
            if (DialogBaseControl == null)
                return;

            // Callbacks abhängen
            Ok = null;
            Cancel = null;
            Yes = null;
            No = null;

            InvokeUICall(() =>
            {
                _dialogHost.HideDialog(DialogBaseControl);
                DialogBaseControl.SetCustomContent(null);
            });

            if (this.OnDialogClosed != null)
                this.OnDialogClosed(this, new EventArgs());
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
