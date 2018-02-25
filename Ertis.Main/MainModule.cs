using Ertis.Infrastructure.Application;
using Ertis.Shared.Models;
using Ertis.Shared.Routing.Contracts;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Main
{
    public class MainModule : EModule
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        public MainModule(IUnityContainer container, IRegionManager regionManager) : base("MainModule", container, regionManager)
        {

        }

        #endregion

        #region Methods

        public override void Initialize()
        {
            this.RegisterView<Views.LeftSidePanel>();
            this.RegisterView<Views.HeaderBar>();
            this.RegisterView<Views.FooterBar>();

            this.RegisterView<Views.MainView>();
            this.RegisterView<Views.DashboardView>();
            
            this.RegisterView<Views.GeneralSettings>();
            this.RegisterView<Views.AppearenceSettings>();
            this.RegisterView<Views.SettingsView>();

            this.IncludeViewToRegion<Views.LeftSidePanel>(RegionNames.LeftSidePanelRegion);
            this.IncludeViewToRegion<Views.HeaderBar>(RegionNames.HeaderBarRegion);
            this.IncludeViewToRegion<Views.FooterBar>(RegionNames.FooterBarRegion);
            this.IncludeViewToRegion<Views.MainView>(RegionNames.MainRegion);

            this.regionManager.RegisterViewWithRegion(RegionNames.NavigationContentRegion, typeof(Views.DashboardView));
        }

        #endregion
    }
}
