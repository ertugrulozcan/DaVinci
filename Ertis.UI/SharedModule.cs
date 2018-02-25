using Ertis.Infrastructure.Application;
using Ertis.Shared.Services;
using Ertis.Shared.Services.Contracts;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Shared
{
    public class SharedModule : EModule
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        public SharedModule(IUnityContainer container, IRegionManager regionManager) : base("SharedModule", container, regionManager)
        {

        }

        #endregion

        #region Methods

        public override void Initialize()
        {
            try
            {
                if (!container.IsRegistered(typeof(IGuiRoutingService)))
                {
                    container.RegisterType<IGuiRoutingService, GuiRoutingService>(new ContainerControlledLifetimeManager());
                }

                if (!container.IsRegistered(typeof(IWindowNavigationService)))
                {
                    container.RegisterType<IWindowNavigationService, WindowNavigationService>(new ContainerControlledLifetimeManager());
                }

                this.RegisterView<Views.MainDockingView>();
                this.RegisterView<Views.NavigationManagerView>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("SharedModule Initialization Error : " + ex.Message);
            }
        }

        #endregion
    }
}
