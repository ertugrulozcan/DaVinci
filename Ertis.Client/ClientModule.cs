using Ertis.Client.Managers;
using Ertis.Client.Managers.Contracts;
using Ertis.Client.Services;
using Ertis.Client.Services.Contracts;
using Ertis.Infrastructure.Application;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Client
{
    public class ClientModule : EModule
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        public ClientModule(IUnityContainer container, IRegionManager regionManager) : base("ClientModule", container, regionManager)
        {

        }

        #endregion

        #region Methods

        public override void Initialize()
        {
            try
            {
                if (!container.IsRegistered(typeof(IAuthenticationService)))
                {
                    container.RegisterType<IErtisWebService, ErtisWebService>(new ContainerControlledLifetimeManager());
                    container.RegisterType<ISessionManager, SessionManager>(new ContainerControlledLifetimeManager());
                    container.RegisterType<IAuthenticationService, AuthenticationService>(new ContainerControlledLifetimeManager());
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ClientModule Initialization Error : " + ex.Message);
            }
        }

        #endregion
    }
}
