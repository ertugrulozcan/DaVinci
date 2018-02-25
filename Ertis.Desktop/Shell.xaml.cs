using Ertis.Infrastructure.Events;
using Ertis.Localization.Services;
using Ertis.Main.ViewModels;
using Ertis.Shared.Events;
using Ertis.Shared.Interfaces;
using Ertis.Shared.ModalWindow.Manager;
using Ertis.Shared.ScreenManagement;
using Ertis.Shared.Services;
using Ertis.Shared.Services.Contracts;
using Ertis.Shared.ViewModels;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
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

namespace Ertis.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Shell : Window, IShell
    {
        private readonly IUnityContainer container;
        private readonly ILocalizationService localizationService;
        private readonly IEventAggregator eventAggregator;

        private WpfScreenManager ScreenManager;

        public LeftSidePanelViewModel LeftSidePanelVM { get; set; }

        public BaseViewModel LeftSidePanelViewModel
        {
            get
            {
                return this.LeftSidePanelVM;
            }
        }

        public Shell(IUnityContainer container, IEventAggregator eventAggregator, ILocalizationService localizationService)
        {
            this.container = container;
            this.eventAggregator = eventAggregator;
            this.localizationService = localizationService;
            
            this.FlowDirection = this.localizationService.UiFlowDirection;

            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            this.ScreenManager = new WpfScreenManager(this);
            this.ScreenManager.OnCurrentScreenChanged += (args) =>
            {
                this.eventAggregator.GetEvent<CurrentScreenChangedEvent>().Publish(args);
                this.InvalidateVisual();
            };
            
            this.eventAggregator.GetEvent<SetBoundsWindowRequestEvent>().Subscribe(SetBoundsWindowRequestEventHandler, ThreadOption.PublisherThread, true);
            this.eventAggregator.GetEvent<LeftSidePanelCollapseRequestEvent>().Subscribe(LeftPanelCollapseRequestEventHandler, ThreadOption.PublisherThread, true);

            // Modal windows
            DialogManager dm = new DialogManager(this, this.DialogManagerPlaceHolder, this.MainRegionContentControl, this.Dispatcher);
            ModalDialogService dialogService = new ModalDialogService(dm);
            this.container.RegisterInstance<IModalDialogService>(dialogService);
        }
        
        private void SetBoundsWindowRequestEventHandler(WpfScreen screen)
        {
            try
            {
                bool isFullScreen = this.WindowState == WindowState.Maximized;

                if (isFullScreen)
                {
                    System.Windows.Rect r = screen.WorkingArea;
                    this.WindowState = WindowState.Normal;
                    this.Left = r.Left;
                    this.Top = r.Top;
                    this.WindowState = WindowState.Maximized;
                }
                else
                {
                    System.Windows.Rect r = screen.WorkingArea;
                    this.Left = this.Left + (screen.WorkingArea.X - WpfScreenManager.Current.CurrentScreen.WorkingArea.X);
                    this.Top = this.Top + (screen.WorkingArea.Y - WpfScreenManager.Current.CurrentScreen.WorkingArea.Y);
                }
            }
            catch
            { }
        }
        
        #region Window Settings

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.BaseWindowBorder.Padding = new Thickness(7);
            }
            else
            {
                this.BaseWindowBorder.Padding = new Thickness(0);
            }
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);

            this.ScreenManager.UpdateCurrentScreen();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
        }

        private void NormalizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Normal;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Left Side Panel

        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // CloseLeftSidePanel
            this.LeftSidePanelVM.IsPanelOpened = false;
        }

        private void LeftPanelCollapseRequestEventHandler(LeftSidePanelCollapseRequestEvent obj)
        {
            // CloseLeftSidePanel
            this.LeftSidePanelVM.IsPanelOpened = false;
        }

        #endregion
    }
}
