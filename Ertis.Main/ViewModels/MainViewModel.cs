using Ertis.Infrastructure.Application;
using Ertis.Shared.ViewModels;
using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Main.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Services

        private readonly IRegionManager regionManager;

        #endregion

        #region Fields



        #endregion

        #region Properties



        #endregion

        #region Constructors

        public MainViewModel(IRegionManager regionManager) : base(Guid.NewGuid().ToString())
        {
            this.regionManager = regionManager;

            this.regionManager.RegisterViewWithRegion(RegionNames.MainViewContentRegion, typeof(Ertis.Shared.Views.NavigationManagerView));
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

        #region Methods



        #endregion

        #region Event Handlers



        #endregion
    }
}
