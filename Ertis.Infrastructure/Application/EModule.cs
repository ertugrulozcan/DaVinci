using Ertis.Infrastructure.Models;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Infrastructure.Application
{
    public abstract class EModule : IModule
    {
        #region Services

        protected readonly IUnityContainer container;
        protected readonly IRegionManager regionManager;

        #endregion

        #region Fields

        private int moduleId = -1; // Registeration'da set edilir.
        private string moduleName;
        private ObservableCollection<View> viewCollection;

        #endregion

        #region Properties

        public int ModuleId
        {
            get
            {
                return moduleId;
            }

            set
            {
                moduleId = value;
            }
        }

        public string ModuleName
        {
            get
            {
                return moduleName;
            }

            private set
            {
                moduleName = value;
            }
        }

        public ObservableCollection<View> ViewCollection
        {
            get
            {
                return viewCollection;
            }
            private set
            {
                viewCollection = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        public EModule(string name, IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;

            this.ModuleName = name;
            this.ViewCollection = new ObservableCollection<View>();
        }

        #endregion

        #region Abstract Methods

        public abstract void Initialize();

        #endregion

        #region Methods

        public void RegisterView<T>() where T : class
        {
            string viewName = typeof(T).FullName;

            try
            {
                container.RegisterType(typeof(Object), typeof(T), viewName);
                this.ViewCollection.Add(new View(viewName, typeof(T)));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("RegisterView error : " + viewName + " - ExceptionMessage : " + ex.Message);
            }
        }

        public void RegisterView<T>(string caption) where T : class
        {
            string viewName = typeof(T).FullName;

            try
            {
                container.RegisterType(typeof(Object), typeof(T), viewName);
                this.ViewCollection.Add(new View(caption, typeof(T)));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("RegisterView error : " + viewName + " - ExceptionMessage : " + ex.Message);
            }
        }

        public void IncludeViewToRegion<ViewType>(string regionName)
        {
            regionManager.RegisterViewWithRegion(regionName, () => this.container.Resolve<ViewType>());
        }

        #endregion
    }
}
