using Ertis.Infrastructure.Application;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Infrastructure
{
    public class InfrastructureModule : EModule
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        public InfrastructureModule(IUnityContainer container, IRegionManager regionManager) : base("InfrastructureModule", container, regionManager)
        {
            
        }

        #endregion

        #region Methods

        public override void Initialize()
        {
            // this.RegisterView<Views.TestView>("Test");
        }

        #endregion
    }
}
