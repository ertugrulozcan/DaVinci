using Ertis.DaVinci.Commands;
using Ertis.DaVinci.Services;
using Ertis.DaVinci.Services.Interfaces;
using Ertis.Shared.Models;
using Ertis.Shared.Routing;
using Ertis.Shared.Services.Contracts;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci
{
    public class DaVinciModule : GuiRoutableModule, ITopMenuPresenter
    {
        private static DaVinciModule self;

        public static DaVinciModule Current
        {
            get
            {
                return self;
            }
        }

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        public DaVinciModule(IUnityContainer container, IRegionManager regionManager) : base("DaVinciModule", container, regionManager)
        {
            self = this;
        }

        #endregion

        #region Methods

        public override void Initialize()
        {
            try
            {
                if (!container.IsRegistered(typeof(ISolutionService)))
                {
                    container.RegisterType<ISolutionService, SolutionService>(new ContainerControlledLifetimeManager());
                }

                this.RegisterView<Views.DesignerView>();
                this.RegisterView<Views.SolutionExplorerView>();
                this.RegisterView<Views.WebBrowserView>();

                this.guiRoutingService.RegisterTopMenuPresenterModule(this);
                base.Initialize();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("DaVinciModule Initialization Error : " + ex.Message);
            }
        }

        public override IViewMenuItem GetViewMenuItem()
        {
            ViewMenuItem parentVmi = new ViewMenuItem(this.ModuleId, this.ModuleName, "PaintBrush");
            int counter = 1;
            
            // Child Vmi's
            parentVmi.Children.Add(new DockableViewMenuItem(this.ModuleId * 100 + counter++, typeof(Views.DesignerView), "Designer", "PictureOutline"));
            parentVmi.Children.Add(new DockableViewMenuItem(this.ModuleId * 100 + counter++, typeof(Views.SolutionExplorerView), "SolutionExplorer", "PictureOutline"));
            parentVmi.Children.Add(new DockableViewMenuItem(this.ModuleId * 100 + counter++, typeof(Views.WebBrowserView), "WebBrowser", "Chrome"));

            return parentVmi;
        }

        public List<IViewMenuItem> GetTopMenuVmiList()
        {
            List<IViewMenuItem> topMenuVmiList = new List<IViewMenuItem>();

            topMenuVmiList.Add(new ModalViewMenuItem(1, typeof(Views.CreateSolutionView), "NewProject", "FileCodeOutline") { ShortTitle = "New" });
            topMenuVmiList.Add(new CommandMenuItem(2, "Open", "FolderOpen", new OpenProjectCommand()));
            topMenuVmiList.Add(new CommandMenuItem(3, "Refresh", "Refresh", new RefreshSolutionCommand()));
            topMenuVmiList.Add(new CommandMenuItem(4, "Save", "FloppyOutline", new SaveProjectCommand()));
            topMenuVmiList.Add(new CommandMenuItem(4, "Export", "Html5", new ExportWebsiteCommand()));

            return topMenuVmiList;
        }

        #endregion
    }
}
