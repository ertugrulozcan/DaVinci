using Ertis.Shared.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ertis.Shared.Models;
using Ertis.Infrastructure.Application;
using Microsoft.Practices.Prism.Regions;
using System.ComponentModel;

namespace Ertis.Shared.Services
{
    public class WindowNavigationService : IWindowNavigationService, INotifyPropertyChanged
    {
        #region Services

        private readonly IRegionManager regionManager;

        #endregion

        #region Fields

        private NavigationWrapperType navigationMode;

        #endregion

        #region Properties

        private Dictionary<string, object> NavigationParamsDictionary { get; set; }

        public NavigationWrapperType NavigationMode
        {
            get
            {
                return this.navigationMode;
            }

            private set
            {
                this.navigationMode = value;
                this.RaisePropertyChanged("NavigationMode");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="regionManager"></param>
        public WindowNavigationService(IRegionManager regionManager)
        {
            this.regionManager = regionManager;

            this.NavigationParamsDictionary = new Dictionary<string, object>();
            this.NavigationMode = NavigationWrapperType.Page;
        }

        #endregion

        #region Methods
        
        public void NavigateView(ViewMenuItemBase vmi, object parameter)
        {
            if (vmi == null)
                return;

            if (!string.IsNullOrEmpty(vmi.ViewName) && parameter != null)
            {
                if (this.NavigationParamsDictionary.ContainsKey(vmi.ViewName))
                    this.NavigationParamsDictionary[vmi.ViewName] = parameter;
                else
                    this.NavigationParamsDictionary.Add(vmi.ViewName, parameter);
            }

            this.NavigateView(vmi);

            if (vmi is ModalViewMenuItem)
            {
                vmi.Navigate<object>(parameter);
            }
        }

        public void NavigateView(ViewMenuItemBase vmi)
        {
            try
            {
                if (vmi is ViewMenuItem)
                {
                    if (this.NavigationMode == NavigationWrapperType.Docking)
                    {
                        //this.regionManager.Regions.Remove(RegionNames.MainViewContentRegion);
                        this.regionManager.RequestNavigate(RegionNames.MainViewContentRegion, typeof(Views.NavigationManagerView).FullName);
                        this.NavigationMode = NavigationWrapperType.Page;
                        this.regionManager.Regions[RegionNames.MainDockingRegion].NavigationService.NavigationFailed -= NavigationService_NavigationFailed;
                    }

                    this.regionManager.RequestNavigate(RegionNames.NavigationContentRegion, vmi.ViewName);
                }
                else if (vmi is DockableViewMenuItem)
                {
                    if (this.NavigationMode == NavigationWrapperType.Page)
                    {
                        this.regionManager.Regions[RegionNames.MainViewContentRegion].NavigationService.NavigationFailed += NavigationService_NavigationFailed1;
                        //this.regionManager.Regions.Remove(RegionNames.MainViewContentRegion);
                        this.regionManager.RequestNavigate(RegionNames.MainViewContentRegion, typeof(Views.MainDockingView).FullName);
                        this.regionManager.Regions[RegionNames.MainViewContentRegion].NavigationService.NavigationFailed -= NavigationService_NavigationFailed1;

                        this.NavigationMode = NavigationWrapperType.Docking;
                        this.regionManager.Regions[RegionNames.MainDockingRegion].NavigationService.NavigationFailed += NavigationService_NavigationFailed;
                    }

                    this.NavigateToWindowDockPanel(vmi as DockableViewMenuItem);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "NavigateView");
            }
        }

        private void NavigationService_NavigationFailed1(object sender, RegionNavigationFailedEventArgs e)
        {

        }

        private void NavigateToWindowDockPanel(DockableViewMenuItem vmi)
        {
            try
            {
                IRegion region = this.regionManager.Regions[RegionNames.MainDockingRegion];
                region.RequestNavigate(vmi.ViewName);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "NavigateToWindowDockPanel");
            }
        }

        public object GetNavigationParameter(string viewName)
        {
            if (this.NavigationParamsDictionary.ContainsKey(viewName))
            {
                return this.NavigationParamsDictionary[viewName];
            }

            return null;
        }

        #endregion

        #region Event Handlers

        private void NavigationService_NavigationFailed(object sender, RegionNavigationFailedEventArgs e)
        {
            System.Windows.MessageBox.Show(e.Error.Message, "NavigationService_NavigationFailed");
            System.Diagnostics.Debug.WriteLine("WindowNavigationService.NavigationFailed : " + e.Error.Message);
        }

        #endregion

        #region RaisePropertyChanged
        
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
