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
using System.Windows.Threading;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using Ertis.Shared.ScreenManagement;
using Ertis.Shared.Events;

namespace Ertis.Shared.Components
{
    /// <summary>
    /// Interaction logic for ScreenSwitcher.xaml
    /// </summary>
    public partial class ScreenSwitcher : UserControl, INotifyPropertyChanged
    {
        #region Services

        private readonly IEventAggregator eventAggregator;

        #endregion

        #region Fields

        private WpfScreenManager screenManager;

        #endregion

        #region Properties

        public WpfScreenManager ScreenManager
        {
            get
            {
                return screenManager;
            }
            set
            {
                screenManager = value;
                this.RaisePropertyChanged("ScreenManager");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public ScreenSwitcher()
        {
            this.eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();

            InitializeComponent();

            this.eventAggregator.GetEvent<CurrentScreenChangedEvent>().Subscribe((args) =>
            {
                Helpers.UIHelper.UiInvoke(DispatcherPriority.DataBind, false, () => { this.RaisePropertyChanged("ScreenManager"); });
            });

            this.ScreenManager = WpfScreenManager.Current;
            this.ScreenManager.DisplaySettingsChanged += ScreenManager_DisplaySettingsChanged;
        }

        #endregion

        #region Event Handlers

        private void ScreenToggleRadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ScreensListBox.SelectedItem is WpfScreen)
            {
                if (this.ScreenManager.CurrentScreen != this.ScreensListBox.SelectedItem)
                {
                    var selectedScreen = this.ScreensListBox.SelectedItem as WpfScreen;

                    this.eventAggregator.GetEvent<SetBoundsWindowRequestEvent>().Publish(selectedScreen);
                }
            }
        }

        private void ScreenManager_DisplaySettingsChanged(object sender, EventArgs e)
        {
            WpfScreenManager.Current.UpdateCurrentScreen();
            this.ScreenManager = WpfScreenManager.Current;
        }

        #endregion

        #region RaisePropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
