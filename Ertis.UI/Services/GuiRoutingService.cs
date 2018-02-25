using Ertis.Infrastructure.Utilities;
using Ertis.Shared.Models;
using Ertis.Shared.Routing.Contracts;
using Ertis.Shared.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Shared.Services
{
    public class GuiRoutingService : IGuiRoutingService
    {
        #region Fields

        private List<IGuiRoutableModule> registeredModules;
        private ObservableRangeCollection<IViewMenuItem> originalVmiList;
        private ReadOnlyObservableCollection<IViewMenuItem> mainViewMenuItems;
        private ObservableRangeCollection<SettingsViewMenuItem> originalSettingsVmiList;
        private ReadOnlyObservableCollection<SettingsViewMenuItem> settingsViewMenuItems;
        private ITopMenuPresenter topMenuPresenterModule;

        #endregion

        #region Properties

        public ReadOnlyObservableCollection<IViewMenuItem> MainViewMenuItems
        {
            get
            {
                return mainViewMenuItems;
            }

            private set
            {
                this.mainViewMenuItems = value;
            }
        }

        public ReadOnlyObservableCollection<SettingsViewMenuItem> SettingsViewMenuItems
        {
            get
            {
                return settingsViewMenuItems;
            }

            private set
            {
                this.settingsViewMenuItems = value;
            }
        }

        public ITopMenuPresenter TopMenuPresenterModule
        {
            get
            {
                return topMenuPresenterModule;
            }

            private set
            {
                this.topMenuPresenterModule = value;
            }
        }

        #endregion

        #region Constructors

        public GuiRoutingService()
        {
            this.registeredModules = new List<IGuiRoutableModule>();

            this.originalVmiList = new ObservableRangeCollection<IViewMenuItem>();
            this.MainViewMenuItems = new ReadOnlyObservableCollection<IViewMenuItem>(this.originalVmiList);

            this.originalSettingsVmiList = new ObservableRangeCollection<SettingsViewMenuItem>();
            this.SettingsViewMenuItems = new ReadOnlyObservableCollection<SettingsViewMenuItem>(originalSettingsVmiList);
        }

        #endregion

        #region Methods

        public void Register(IGuiRoutableModule guiRoutableModule)
        {
            if (this.registeredModules.Contains(guiRoutableModule))
                return;

            this.registeredModules.Add(guiRoutableModule);
            this.originalVmiList.Add(guiRoutableModule.ViewMenuItem);

            if (guiRoutableModule is HasSettingsModule)
            {
                var hasSettingsModule = (guiRoutableModule as HasSettingsModule);
                this.originalSettingsVmiList.AddRange(hasSettingsModule.GetSettingsViewMenuItemList());
            }
        }

        public void RegisterTopMenuPresenterModule(ITopMenuPresenter presenterModule)
        {
            this.TopMenuPresenterModule = presenterModule;
        }

        #endregion
    }
}
