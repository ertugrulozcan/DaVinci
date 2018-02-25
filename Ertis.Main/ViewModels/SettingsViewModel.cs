using Ertis.Infrastructure.Application;
using Ertis.Infrastructure.Events;
using Ertis.Infrastructure.Utilities;
using Ertis.Localization.Services;
using Ertis.Shared.ModalWindow.Contracts;
using Ertis.Shared.Models;
using Ertis.Shared.Services.Contracts;
using Ertis.Shared.ViewModels;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFLocalizeExtension.Engine;

namespace Ertis.Main.ViewModels
{
    public class SettingsViewModel : BaseViewModel, ICustomOkCancelControl
    {
        #region Services

        private readonly IGuiRoutingService guiRoutingService;
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;

        #endregion

        #region Fields

        private ObservableRangeCollection<SettingsViewMenuItem> settingsVmiCollection;
        private SettingsViewMenuItem selectedSettingsVMI;

        #endregion

        #region Properties

        public ObservableRangeCollection<SettingsViewMenuItem> SettingsVmiCollection
        {
            get
            {
                return settingsVmiCollection;
            }

            set
            {
                this.settingsVmiCollection = value;
                this.RaisePropertyChanged("SettingsVmiCollection");
            }
        }

        public SettingsViewMenuItem SelectedSettingsVMI
        {
            get
            {
                return selectedSettingsVMI;
            }

            set
            {
                this.selectedSettingsVMI = value;
                this.RaisePropertyChanged("SelectedSettingsVMI");

                if (this.SelectedSettingsVMI != null)
                    this.regionManager.RequestNavigate(RegionNames.SettingsViewTabContentRegion, this.SelectedSettingsVMI.ViewName);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsViewModel(IRegionManager regionManager, IGuiRoutingService guiRoutingService, IEventAggregator eventAggregator) : base(Guid.NewGuid().ToString())
        {
            this.regionManager = regionManager;
            this.guiRoutingService = guiRoutingService;
            this.eventAggregator = eventAggregator;

            var settingsVmiList = new List<SettingsViewMenuItem>();
            settingsVmiList.Add(new SettingsViewMenuItem(3001, typeof(Views.GeneralSettings), "General", "Cog"));
            settingsVmiList.Add(new SettingsViewMenuItem(3003, typeof(Views.AppearenceSettings), "Appearence", "PaintBrush"));
            settingsVmiList.AddRange(this.guiRoutingService.SettingsViewMenuItems);
            this.SettingsVmiCollection = new ObservableRangeCollection<SettingsViewMenuItem>(settingsVmiList);

            this.eventAggregator.GetEvent<LanguageChangedEvent>().Subscribe(OnLanguageChangedEvent);
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

        private void OnLanguageChangedEvent(CultureInfo obj)
        {
            this.RaisePropertyChanged("SelectedSettingsVMI");
        }

        #endregion

        #region ICustomOkCancelControl

        public bool OkClicked()
        {
            return true;
        }

        public bool CancelClicked()
        {
            return true;
        }

        public object CustControlNavParams
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Dispose

        public new void Dispose()
        {
            this.eventAggregator.GetEvent<LanguageChangedEvent>().Unsubscribe(OnLanguageChangedEvent);

            base.Dispose();
        }

        #endregion
    }
}
