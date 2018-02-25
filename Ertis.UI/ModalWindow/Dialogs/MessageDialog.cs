using Ertis.Shared.ModalWindow.Contracts;
using Ertis.Shared.ModalWindow.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Ertis.Shared.ModalWindow.Dialogs
{
    class MessageDialog : DialogBase, IMessageDialog
    {
        private TextBlock _messageTextBlock;

        public MessageDialog(IDialogHost dialogHost, DialogMode dialogMode, string message, Dispatcher dispatcher) : base(dialogHost, dialogMode, dispatcher)
        {
            InvokeUICall(() =>
            {
                _messageTextBlock = new TextBlock
                {
                    Text = message,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    TextWrapping = TextWrapping.Wrap,
                    //Foreground = Ertis.Themes.ThemeManager.GetElementFromResource("ContrastBrush5") as SolidColorBrush
                };
                SetContent(_messageTextBlock);
            });
        }

        #region Implementation of IMessageDialog

        public string Message
        {
            get
            {
                var text = string.Empty;
                InvokeUICall(() => text = _messageTextBlock.Text);
                return text;
            }
            set
            {
                InvokeUICall(() => _messageTextBlock.Text = value);
            }
        }

        #endregion
    }
}
