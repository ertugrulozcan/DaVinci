using Ertis.Infrastructure.Events;
using Ertis.Shared.Helpers;
using Ertis.Shared.ModalWindow.Dialogs;
using Ertis.Shared.ModalWindow.Manager;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;

namespace Ertis.Shared.ModalWindow.Views
{
    /// <summary>
    /// Interaction logic for DialogBaseControl.xaml
    /// </summary>
    partial class DialogBaseControl : UserControl, INotifyPropertyChanged, IDisposable
    {
        #region Services

        private readonly IEventAggregator eventAggregator;

        #endregion

        #region Fields

        private readonly DialogBase dialog;
        private bool isCloseButtonVisible;
        private string caption;
        private VerticalAlignment verticalDialogAlignment = VerticalAlignment.Center;
        private HorizontalAlignment horizontalDialogAlignment = HorizontalAlignment.Center;

        #endregion

        #region Properties

        public string Caption
        {
            get
            {
                return caption;
            }

            private set
            {
                caption = value;
                this.OnPropertyChanged("Caption");
                this.OnPropertyChanged("CaptionVisibility");
            }
        }

        public Visibility CaptionVisibility
        {
            get
            {
                return string.IsNullOrWhiteSpace(Caption)
                    ? Visibility.Collapsed
                    : Visibility.Visible;
            }
        }

        public VerticalAlignment VerticalDialogAlignment
        {
            get { return verticalDialogAlignment; }
            set
            {
                verticalDialogAlignment = value;
                OnPropertyChanged("VerticalDialogAlignment");
            }
        }

        public HorizontalAlignment HorizontalDialogAlignment
        {
            get { return horizontalDialogAlignment; }
            set
            {
                horizontalDialogAlignment = value;
                OnPropertyChanged("HorizontalDialogAlignment");
            }
        }

        public bool IsCloseButtonVisible
        {
            get
            {
                return isCloseButtonVisible;
            }

            set
            {
                isCloseButtonVisible = value;
                OnPropertyChanged("IsCloseButtonVisible");

                if (isCloseButtonVisible)
                    this.CloseButton.Visibility = Visibility.Visible;
                else
                    this.CloseButton.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="originalContent"></param>
        /// <param name="dialog"></param>
        /// <param name="isCloseButtonVisible"></param>
        public DialogBaseControl(FrameworkElement originalContent, DialogBase dialog, bool isCloseButtonVisible)
        {
            this.Caption = Localization.LocalizationUtility.Convert(dialog.Caption);

            InitializeComponent();

            this.IsCloseButtonVisible = isCloseButtonVisible;
            
            //this.BackgroundImageHolder.Source = originalContent.CaptureImage();
            this.BackgroundImageHolder.Source = originalContent.DrawToImage();

            this.dialog = dialog;
            this.CreateButtons();

            this.eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            this.eventAggregator.GetEvent<LanguageChangedEvent>().Subscribe(OnLanguageChangedEvent);
        }

        #endregion

        #region Event Handlers

        private void OnLanguageChangedEvent(CultureInfo obj)
        {
            this.Caption = Localization.LocalizationUtility.Convert(dialog.Caption);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (dialog.Cancel != null)
                dialog.Cancel();
            else
                dialog.Close();
        }

        #endregion

        #region Methods

        public void SetCustomContent(object content)
        {
            CustomContent.Content = content;
        }

        private void CreateButtons()
        {
            switch (dialog.Mode)
            {
                case DialogMode.None:
                    this.ButtonsGrid.Visibility = Visibility.Collapsed;
                    break;
                case DialogMode.Ok:
                    AddOkButton();
                    break;
                case DialogMode.Cancel:
                    {
                        if (!this.IsCloseButtonVisible)
                            AddCancelButton();
                    }
                    break;
                case DialogMode.OkCancel:
                    AddOkButton();
                    AddCancelButton();
                    break;
                case DialogMode.YesNo:
                    AddYesButton();
                    AddNoButton();
                    break;
                case DialogMode.YesNoCancel:
                    AddYesButton();
                    AddNoButton();
                    AddCancelButton();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void AddNoButton()
        {
            AddButton(dialog.NoText, GetCallback(dialog.No, DialogResultState.No), false, true, "CanNo");
        }

        public void AddYesButton()
        {
            AddButton(dialog.YesText, GetCallback(dialog.Yes, DialogResultState.Yes), true, false, "CanYes");
        }

        public void AddCancelButton()
        {
            AddButton(dialog.CancelText, GetCallback(dialog.Cancel, DialogResultState.Cancel), false, true, "CanCancel");
        }

        public void AddOkButton()
        {
            AddButton(dialog.OkText, GetCallback(dialog.Ok, DialogResultState.Ok), true, true, "CanOk");
        }

        private void AddButton(string buttonText, Action callback, bool isDefault, bool isCancel, string bindingPath)
        {
            var btn = new Button
            {
                Content = buttonText,
                MinWidth = 80,
                MaxWidth = 150,
                IsDefault = isDefault,
                IsCancel = isCancel,
                Margin = new Thickness(5)
            };

            var enabledBinding = new Binding(bindingPath) { Source = dialog };
            btn.SetBinding(IsEnabledProperty, enabledBinding);

            btn.Click += (s, e) => callback();

            ButtonsGrid.Columns++;
            ButtonsGrid.Children.Add(btn);
        }

        internal void RemoveButtons()
        {
            ButtonsGrid.Children.Clear();
        }

        private Action GetCallback(Action dialogCallback, DialogResultState result)
        {
            dialog.Result = result;
            Action callback = () =>
            {
                if (dialogCallback != null)
                    dialogCallback();
                if (dialog.CloseBehavior == DialogCloseBehavior.AutoCloseOnButtonClick)
                    dialog.Close();
            };

            return callback;
        }

        #endregion

        #region RaisePropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Disposing

        public void Dispose()
        {
            this.eventAggregator.GetEvent<LanguageChangedEvent>().Unsubscribe(OnLanguageChangedEvent);
        }

        #endregion
    }
}
