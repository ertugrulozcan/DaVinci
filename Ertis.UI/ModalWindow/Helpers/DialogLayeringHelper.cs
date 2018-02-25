using Ertis.Shared.ModalWindow.Contracts;
using Ertis.Shared.ModalWindow.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Ertis.Shared.ModalWindow.Helpers
{
    class DialogLayeringHelper : IDialogHost
    {
        public DialogLayeringHelper(ContentControl parent, ContentControl layer, ContentControl region)
        {
            this._parent = parent;
            this.layer = layer;
            this.region = region;
        }

        private readonly ContentControl _parent;
        private readonly ContentControl layer;
        private readonly ContentControl region;
        private readonly List<object> _layerStack = new List<object>();

        public bool HasDialogLayers { get { return _layerStack.Any(); } }

        #region Implementation of IDialogHost

        public void ShowDialog(DialogBaseControl dialog)
        {
            _layerStack.Add(this.layer.Content);
            this.layer.Content = dialog;
        }

        public void HideDialog(DialogBaseControl dialog)
        {
            if (this.layer.Content == dialog)
            {
                var oldContent = _layerStack.Last();
                _layerStack.Remove(oldContent);
                this.layer.Content = oldContent;
            }
            else
                _layerStack.Remove(dialog);

            if (_layerStack.Count == 0)
                this.region.Visibility = Visibility.Visible;
        }

        public FrameworkElement GetCurrentContent()
        {
            return this._parent;
        }

        public FrameworkElement GetCurrentRegion()
        {
            return this.region;
        }

        #endregion
    }
}
