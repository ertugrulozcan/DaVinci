using Ertis.Infrastructure.Application;
using Ertis.Shared.ViewModels;
using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Shared.ViewModels
{
    public class NavigationManagerViewModel : BaseViewModel
    {
        #region Services

        private readonly IRegionManager regionManager;

        #endregion

        #region Fields



        #endregion

        #region Properties



        #endregion

        #region Constructors

        public NavigationManagerViewModel(IRegionManager regionManager) : base(Guid.NewGuid().ToString())
        {
            this.regionManager = regionManager;
        }

        #endregion

        #region Methods

        protected override void OnNavigatedFrom()
        {

        }

        protected override void OnNavigatedTo(object parameter)
        {

        }

        #endregion

        #region Event Handlers



        #endregion
    }
}
