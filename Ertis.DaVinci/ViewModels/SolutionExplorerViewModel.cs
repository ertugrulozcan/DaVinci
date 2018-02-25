using Ertis.DaVinci.Services.Interfaces;
using Ertis.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.ViewModels
{
    public class SolutionExplorerViewModel : BaseViewModel
    {
        #region Services

        private readonly ISolutionService solutionService;

        #endregion

        #region Properties

        public ISolutionService SolutionService
        {
            get
            {
                return this.solutionService;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="guid"></param>
        public SolutionExplorerViewModel(ISolutionService solutionService) : base(Guid.NewGuid().ToString())
        {
            this.solutionService = solutionService;
        }

        #endregion

        #region Navigate Methods

        protected override void OnNavigatedFrom()
        {

        }

        protected override void OnNavigatedTo(object parameter)
        {

        }

        #endregion
    }
}
