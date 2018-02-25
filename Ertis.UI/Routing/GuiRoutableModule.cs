using Ertis.Infrastructure.Application;
using Ertis.Shared.Routing.Contracts;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ertis.Shared.Services.Contracts;
using Ertis.Shared.Models;
using Microsoft.Practices.ServiceLocation;

namespace Ertis.Shared.Routing
{
    public abstract class GuiRoutableModule : EModule, IGuiRoutableModule
    {
        protected readonly IGuiRoutingService guiRoutingService;

        public IViewMenuItem ViewMenuItem { get; private set; }

        public GuiRoutableModule(string name, IUnityContainer container, IRegionManager regionManager) : base(name, container, regionManager)
        {
            this.guiRoutingService = ServiceLocator.Current.GetInstance<IGuiRoutingService>();
        }

        public override void Initialize()
        {
            this.ViewMenuItem = this.GetViewMenuItem();

            this.guiRoutingService.Register(this);
        }

        public abstract IViewMenuItem GetViewMenuItem();
    }
}
