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
using FontAwesome.WPF;
using Microsoft.Practices.Prism.Commands;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using Ertis.Shared.Interfaces;

namespace Ertis.Shared.Components
{
    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : UserControl, INotifyPropertyChanged
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        static extern bool EnableWindow(IntPtr hWnd, bool bEnable);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        #region Fields

        private ImageAwesome fontAwesomeIcon;
        private Window messageBoxWindow;

        #endregion

        #region Dependency Properties


        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(CustomMessageBox), new PropertyMetadata("Message"));


        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(CustomMessageBox), new PropertyMetadata(string.Empty));


        public MessageTypes MessageType
        {
            get { return (MessageTypes)GetValue(MessageTypeProperty); }
            set { SetValue(MessageTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MessageType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageTypeProperty =
            DependencyProperty.Register("MessageType", typeof(MessageTypes), typeof(CustomMessageBox), new PropertyMetadata(MessageTypes.Blank, OnMessageTypeChangedCallback));

        public DialogTypes DialogType
        {
            get { return (DialogTypes)GetValue(DialogTypeProperty); }
            set { SetValue(DialogTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DialogType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DialogTypeProperty =
            DependencyProperty.Register("DialogType", typeof(DialogTypes), typeof(CustomMessageBox), new PropertyMetadata(DialogTypes.Ok));


        #endregion

        #region Properties

        public ImageAwesome AwesomeIcon
        {
            get
            {
                return this.fontAwesomeIcon;
            }
            set
            {
                this.fontAwesomeIcon = value;
                this.RaisePropertyChanged("AwesomeIcon");
            }
        }

        #endregion

        #region Commands

        public DelegateCommand OkButtonCommand { get; set; }
        public DelegateCommand CancelButtonCommand { get; set; }
        public DelegateCommand YesButtonCommand { get; set; }
        public DelegateCommand NoButtonCommand { get; set; }

        #endregion

        #region Events

        public event EventHandler YesButtonAction;
        public event EventHandler NoButtonAction;
        public event EventHandler OkButtonAction;
        public event EventHandler CancelButtonAction;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        private CustomMessageBox()
        {
            InitializeComponent();

            this.OkButtonCommand = new DelegateCommand(ExecuteOkButtonCommand);
            this.CancelButtonCommand = new DelegateCommand(ExecuteCancelButtonCommand);
            this.YesButtonCommand = new DelegateCommand(ExecuteYesButtonCommand);
            this.NoButtonCommand = new DelegateCommand(ExecuteNoButtonCommand);
        }

        private static CustomMessageBox Create(string message)
        {
            return new CustomMessageBox() { Message = message, Title = Localization.LocalizationUtility.Convert("Message"), MessageType = MessageTypes.Blank, DialogType = DialogTypes.Ok };
        }

        private static CustomMessageBox Create(string title, string message)
        {
            return new CustomMessageBox() { Message = message, Title = title, MessageType = MessageTypes.Blank, DialogType = DialogTypes.Ok };
        }

        private static CustomMessageBox Create(string title, string message, MessageTypes messageType)
        {
            return new CustomMessageBox() { Message = message, Title = title, MessageType = messageType, DialogType = DialogTypes.Ok };
        }

        private static CustomMessageBox Create(string title, string message, DialogTypes dialogType)
        {
            return new CustomMessageBox() { Message = message, Title = title, MessageType = MessageTypes.Blank, DialogType = dialogType };
        }

        private static CustomMessageBox Create(string title, string message, MessageTypes messageType, DialogTypes dialogType)
        {
            return new CustomMessageBox() { Message = message, Title = title, MessageType = messageType, DialogType = dialogType };
        }

        #endregion

        #region Methods

        public static CustomMessageBox Show(string message)
        {
            var messageBox = Create(message);
            messageBox.ShowDialog();

            return messageBox;
        }

        public static CustomMessageBox Show(string title, string message)
        {
            var messageBox = Create(title, message);
            messageBox.ShowDialog();

            return messageBox;
        }

        public static CustomMessageBox Show(string title, string message, MessageTypes messageType)
        {
            var messageBox = Create(title, message, messageType);
            messageBox.ShowDialog();

            return messageBox;
        }

        public static CustomMessageBox Show(string title, string message, DialogTypes dialogType)
        {
            var messageBox = Create(title, message, dialogType);
            messageBox.ShowDialog();

            return messageBox;
        }

        public static CustomMessageBox Show(string message, MessageTypes messageType, DialogTypes dialogType)
        {
            var messageBox = Create(Localization.LocalizationUtility.Convert("Message"), message, messageType, dialogType);
            messageBox.ShowDialog();

            return messageBox;
        }

        public static CustomMessageBox Show(string title, string message, MessageTypes messageType, DialogTypes dialogType)
        {
            var messageBox = Create(title, message, messageType, dialogType);
            messageBox.ShowDialog();

            return messageBox;
        }

        private void ShowDialog()
        {
            try
            {
                this.messageBoxWindow = new Window();
                System.Windows.Shell.WindowChrome.SetWindowChrome(this.messageBoxWindow, new System.Windows.Shell.WindowChrome()
                {
                    CaptionHeight = 180,
                    ResizeBorderThickness = new Thickness(7),
                    UseAeroCaptionButtons = false
                });

                this.messageBoxWindow.Title = this.Title;
                this.messageBoxWindow.Content = this;
                this.messageBoxWindow.ResizeMode = ResizeMode.NoResize;
                this.messageBoxWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                this.messageBoxWindow.Topmost = true;
                this.messageBoxWindow.ShowInTaskbar = false;
                this.messageBoxWindow.Width = 390;
                this.messageBoxWindow.Height = 180;

                var shell = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IShell>();
                if (shell != null && (shell as Window).IsInitialized)
                {
                    IntPtr handle = (new System.Windows.Interop.WindowInteropHelper(shell as Window)).Handle;
                    this.messageBoxWindow.Owner = shell as Window;
                    WindowInteropHelper helper = new WindowInteropHelper(this.messageBoxWindow);
                    EnableWindow(handle, false);
                    this.messageBoxWindow.Closing += new System.ComponentModel.CancelEventHandler(Window_Closing);
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                if (this.messageBoxWindow != null)
                    this.messageBoxWindow.Show();
            }
        }

        void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var shell = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IShell>();
            this.messageBoxWindow.Closing -= new System.ComponentModel.CancelEventHandler(Window_Closing);
            IntPtr handle = (new System.Windows.Interop.WindowInteropHelper(this.messageBoxWindow)).Handle;
            IntPtr ownerhandle = (new System.Windows.Interop.WindowInteropHelper(shell as Window)).Handle;
            EnableWindow(handle, false);
            EnableWindow(ownerhandle, true);
        }

        private void CloseDialog()
        {
            if (this.messageBoxWindow != null)
                this.messageBoxWindow.Close();

            this.UnsubscribeAllDelegates(this.OkButtonAction);
            this.UnsubscribeAllDelegates(this.NoButtonAction);
            this.UnsubscribeAllDelegates(this.YesButtonAction);
            this.UnsubscribeAllDelegates(this.NoButtonAction);
            this.UnsubscribeAllDelegates(this.CancelButtonAction);
        }

        private void UnsubscribeAllDelegates(EventHandler handler)
        {
            if (handler != null)
            {
                Delegate[] delegationList = handler.GetInvocationList();
                foreach (var delegation in delegationList)
                    handler -= (delegation as EventHandler);
            }
        }

        #endregion

        #region Callback Method

        private static void OnMessageTypeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomMessageBox self = d as CustomMessageBox;

            var icon = new ImageAwesome();
            switch (self.MessageType)
            {
                case MessageTypes.Blank:
                    self.AwesomeIcon = null;
                    break;
                case MessageTypes.Information:
                    icon.Icon = FontAwesomeIcon.InfoCircle;
                    icon.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x2B, 0x9B, 0xE5));
                    self.AwesomeIcon = icon;
                    break;
                case MessageTypes.Question:
                    icon.Icon = FontAwesomeIcon.QuestionCircle;
                    icon.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xFC, 0xE1, 0x00));
                    self.AwesomeIcon = icon;
                    break;
                case MessageTypes.Warning:
                    icon.Icon = FontAwesomeIcon.ExclamationTriangle;
                    icon.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x09, 0xB4, 0x8B));
                    self.AwesomeIcon = icon;
                    break;
                case MessageTypes.Error:
                    icon.Icon = FontAwesomeIcon.ExclamationCircle;
                    icon.Foreground = new SolidColorBrush(Colors.Red);
                    self.AwesomeIcon = icon;
                    break;
                default:
                    self.AwesomeIcon = null;
                    break;
            }
        }

        #endregion

        #region Command Methods

        private void ExecuteOkButtonCommand()
        {
            if (this.OkButtonAction != null)
                this.OkButtonAction(this, new EventArgs());

            this.CloseDialog();
        }

        private void ExecuteCancelButtonCommand()
        {
            if (this.CancelButtonAction != null)
                this.CancelButtonAction(this, new EventArgs());

            this.CloseDialog();
        }

        private void ExecuteYesButtonCommand()
        {
            if (this.YesButtonAction != null)
                this.YesButtonAction(this, new EventArgs());

            this.CloseDialog();
        }

        private void ExecuteNoButtonCommand()
        {
            if (this.NoButtonAction != null)
                this.NoButtonAction(this, new EventArgs());

            this.CloseDialog();
        }

        #endregion

        #region Enums

        public enum MessageTypes
        {
            Blank,
            Information,
            Question,
            Warning,
            Error,
        }

        public enum DialogTypes
        {
            None,
            Ok,
            OkCancel,
            YesNo,
            YesNoCancel,
        }

        #endregion

        #region RaisePropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    public class CustomMessageBoxButtonsTemplateSelector : DataTemplateSelector
    {
        public DataTemplate BlankTemplate { get; set; }
        public DataTemplate OkTemplate { get; set; }
        public DataTemplate OkCancelTemplate { get; set; }
        public DataTemplate YesNoTemplate { get; set; }
        public DataTemplate YesNoCancelTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is CustomMessageBox.DialogTypes)
            {
                switch ((CustomMessageBox.DialogTypes)item)
                {
                    case CustomMessageBox.DialogTypes.None:
                        return BlankTemplate;
                    case CustomMessageBox.DialogTypes.Ok:
                        return OkTemplate;
                    case CustomMessageBox.DialogTypes.OkCancel:
                        return OkCancelTemplate;
                    case CustomMessageBox.DialogTypes.YesNo:
                        return YesNoTemplate;
                    case CustomMessageBox.DialogTypes.YesNoCancel:
                        return YesNoCancelTemplate;
                    default:
                        return BlankTemplate;
                }
            }
            else
                return base.SelectTemplate(item, container);
        }
    }
}
