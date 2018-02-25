using DevExpress.Xpf.Docking;
using Ertis.DaVinci;
using Ertis.Infrastructure;
using Ertis.Infrastructure.Application;
using Ertis.Infrastructure.Events;
using Ertis.Infrastructure.Services;
using Ertis.Infrastructure.Services.Contracts;
using Ertis.Localization.Services;
using Ertis.Main;
using Ertis.Shared;
using Ertis.Shared.Helpers;
using Ertis.Shared.Interfaces;
using Ertis.Shared.RegionAdapters;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Ertis.Desktop
{
    public class Bootstrapper : UnityBootstrapper
    {
        #region Services

        public IModuleManager moduleManager
        {
            get { return this.Container.Resolve<IModuleManager>(); }
        }

        public IServiceLocator ServiceLocater
        {
            get { return this.Container.Resolve<IServiceLocator>(); }
        }

        public IEventAggregator eventAggregator
        {
            get { return this.Container.Resolve<IEventAggregator>(); }
        }

        #endregion

        #region Properties

        public SplashScreen SplashScreen { get; private set; }
        
        public Shell MainWindow { get; private set; }

        public List<Type> ModuleTypes = new List<Type>();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public Bootstrapper()
        {
            this.ModuleTypes.Add(typeof(SharedModule));
            this.ModuleTypes.Add(typeof(InfrastructureModule));
            this.ModuleTypes.Add(typeof(DaVinciModule));
            this.ModuleTypes.Add(typeof(MainModule));
        }

        #endregion

        #region Methods

        protected override DependencyObject CreateShell()
        {
            this.SplashScreen = this.Container.Resolve<SplashScreen>();
            this.MainWindow = this.Container.Resolve<Shell>();
            
            var regionManager = this.Container.Resolve<IRegionManager>();
            RegionManager.SetRegionManager(this.MainWindow, regionManager);
            RegionManager.UpdateRegions();
            
            this.eventAggregator.GetEvent<LeftSidePanelLoadedEvent>().Subscribe(OnLeftSidePanelLoaded);

            this.SplashScreen.Show();

            return this.MainWindow;
        }

        private void SetLeftSidePanel()
        {
            var regionManager = this.Container.Resolve<IRegionManager>();
            //var leftSidePanelView = regionManager.Regions[RegionNames.LeftSidePanelRegion].GetView(typeof(Main.Views.LeftSidePanel).FullName) as UserControl;
            var leftSidePanelView = regionManager.Regions[RegionNames.LeftSidePanelRegion].ActiveViews.First() as UserControl;
            var leftSidePanelVM = leftSidePanelView.DataContext as Main.ViewModels.LeftSidePanelViewModel;
            this.MainWindow.LeftSidePanelVM = leftSidePanelVM;
        }

        protected override void ConfigureContainer()
        {
            try
            {
                this.Container.RegisterType<ISettingsService, SettingsService>(new ContainerControlledLifetimeManager());
                this.Container.RegisterType<ILocalizationService, LocalizationService>(new ContainerControlledLifetimeManager());
                this.Container.RegisterType<IShell, Shell>(new ContainerControlledLifetimeManager());

                base.ConfigureContainer();
            }
            catch (Exception ex)
            {

            }
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

            ModuleCatalog moduleCatalog = (ModuleCatalog)this.ModuleCatalog;
            foreach (var module in this.ModuleTypes)
            {
                moduleCatalog.AddModule(module);
            }
        }

        protected override IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
        {
            IRegionBehaviorFactory factory = base.ConfigureDefaultRegionBehaviors();
            return factory;
        }

        protected override void InitializeModules()
        {
            int moduleId = 101;

            foreach (var moduleType in this.ModuleTypes)
            {
                IModule module = this.Container.Resolve(moduleType) as IModule;
                if (module is EModule)
                {
                    (module as EModule).ModuleId = moduleId++;
                    module.Initialize();
                    Infrastructure.Application.ModuleManager.Current.ModuleList.Add(module as EModule);
                }
            }

            this.AllModulesInitialized();
        }

        private void AllModulesInitialized()
        {
            // ApplyLastSettings
            var settingService = ServiceLocator.Current.GetInstance<ISettingsService>();
            var lastSettings = settingService.GetLastSettings();
            this.ApplySettings(lastSettings);

            this.eventAggregator.GetEvent<AllModulesInitializedEvent>();
            
            this.SplashScreen.Close();
            this.MainWindow.Show();
            this.MainWindow.WindowState = WindowState.Maximized;
        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            RegionAdapterMappings mappings = base.ConfigureRegionAdapterMappings();
            if (mappings != null)
            {
                mappings.RegisterMapping(typeof(LayoutGroup), ServiceLocator.Current.GetInstance<DockingRegionAdapter>());
            }

            return mappings;
        }

        private void ApplySettings(AppSettings settings)
        {
            var localizationService = ServiceLocator.Current.GetInstance<ILocalizationService>();
            
            // Localization
            localizationService.UpdateTranslationResources(settings.Culture, new HashSet<string>());

            // Theme
            Ertis.Themes.ThemeManager.Current.ChangeTheme(settings.ThemeName);
        }

        #endregion

        #region Event Handlers

        private void OnLeftSidePanelLoaded(LeftSidePanelLoadedEvent obj)
        {
            this.SetLeftSidePanel();
            this.eventAggregator.GetEvent<LeftSidePanelLoadedEvent>().Unsubscribe(OnLeftSidePanelLoaded);
        }
        
        #endregion
    }
}
