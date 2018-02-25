using Ertis.Infrastructure.Application;
using Ertis.Shared.Models;
using Ertis.Shared.Services.Contracts;
using Ertis.Shared.ViewModels;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Ertis.Main.ViewModels
{
    public class LeftSidePanelViewModel : BaseViewModel
    {
        #region Services
        
        private readonly IGuiRoutingService guiRoutingService;
        private readonly IRegionManager regionManager;

        public IWindowNavigationService WindowNavigationService { get; set; }
        
        #endregion

        #region Fields

        private bool isPanelOpened;
        private ObservableCollection<IViewMenuItem> mainViewMenuItems;
        private IViewMenuItem selectedVMI;
        private IViewMenuItem hiddenSelectedVMI;
        private ViewMenuItem dashboardVMI;
        private ModalViewMenuItem settingsVMI;

        #endregion

        #region Properties

        public bool IsPanelOpened
        {
            get
            {
                return isPanelOpened;
            }

            set
            {
                if (this.isPanelOpened == value)
                    return;

                this.isPanelOpened = value;
                this.RaisePropertyChanged("IsPanelOpened");
                this.RaisePropertyChanged("PanelOpenedComponentsVisibility");
                this.RaisePropertyChanged("PanelClosedComponentsVisibility");

                if (this.IsPanelOpened)
                    this.SelectedVMI = this.HiddenSelectedVMI;
                else
                    this.SelectedVMI = null;
            }
        }

        public Visibility PanelOpenedComponentsVisibility
        {
            get
            {
                if (this.IsPanelOpened)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }

        public Visibility PanelClosedComponentsVisibility
        {
            get
            {
                if (this.IsPanelOpened)
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
            }
        }

        public ObservableCollection<IViewMenuItem> MainViewMenuItems
        {
            get
            {
                return mainViewMenuItems;
            }

            private set
            {
                this.mainViewMenuItems = value;
                this.RaisePropertyChanged("MainViewMenuItems");
            }
        }

        public IViewMenuItem SelectedVMI
        {
            get
            {
                return selectedVMI;
            }

            set
            {
                if (this.selectedVMI == value)
                    return;
                
                this.selectedVMI = value;
                this.RaisePropertyChanged("SelectedVMI");

                if (value != null)
                {
                    this.HiddenSelectedVMI = value;

                    if (!this.IsPanelOpened)
                        this.IsPanelOpened = true;
                }
            }
        }

        public IViewMenuItem HiddenSelectedVMI
        {
            get
            {
                return hiddenSelectedVMI;
            }

            private set
            {
                this.hiddenSelectedVMI = value;
                this.RaisePropertyChanged("HiddenSelectedVMI");
            }
        }

        public ViewMenuItem DashboardVMI
        {
            get
            {
                if (dashboardVMI == null)
                {
                    this.dashboardVMI = new ViewMenuItem(1001, typeof(Views.DashboardView), "Dashboard", "Home");
                }

                return dashboardVMI;
            }
        }

        public ModalViewMenuItem SettingsVMI
        {
            get
            {
                if (settingsVMI == null)
                {
                    this.settingsVMI = new ModalViewMenuItem(3000, typeof(Views.SettingsView), "Settings", "Cogs");
                    this.settingsVMI.DialogClosed += SettingsDialogClosed;
                }

                return settingsVMI;
            }
        }
        
        #endregion

        #region Commands

        public DelegateCommand LeftSidePanelCommand { get; set; }
        
        #endregion

        #region Constructors

        public LeftSidePanelViewModel(IGuiRoutingService guiRoutingService, IRegionManager regionManager, IWindowNavigationService windowNavigationService) : base(Guid.NewGuid().ToString())
        {
            this.guiRoutingService = guiRoutingService;
            this.regionManager = regionManager;
            this.WindowNavigationService = windowNavigationService;
            
            this.LeftSidePanelCommand = new DelegateCommand(LeftSidePanelCommandExecute);

            this.MainViewMenuItems = new ObservableCollection<IViewMenuItem>(this.guiRoutingService.MainViewMenuItems);
        }

        #endregion

        #region Navigation Methods

        protected override void OnNavigatedTo(object parameter)
        {

        }

        protected override void OnNavigatedFrom()
        {

        }

        #endregion

        #region Event Handlers
        
        private void SettingsDialogClosed(object sender, EventArgs e)
        {
            this.regionManager.Regions.Remove(RegionNames.SettingsViewTabContentRegion);
        }

        #endregion

        #region Command Methods

        private void LeftSidePanelCommandExecute()
        {
            this.IsPanelOpened = !this.IsPanelOpened;
        }

        #endregion
    }
}
