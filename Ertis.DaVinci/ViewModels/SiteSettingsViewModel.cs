using Ertis.DaVinci.Models;
using Ertis.DaVinci.Services.Interfaces;
using Ertis.Shared.ModalWindow.Contracts;
using Ertis.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.ViewModels
{
    public class SiteSettingsViewModel : BaseViewModel, ICustomOkCancelControl
    {
        #region Services

        private readonly ISolutionService solutionService;

        #endregion

        #region Fields

        private SiteSettings currentSiteSettings;

        #endregion

        #region Properties

        public SiteSettings CurrentSiteSettings
        {
            get
            {
                return currentSiteSettings;
            }

            set
            {
                this.currentSiteSettings = value;
                this.RaisePropertyChanged("CurrentSiteSettings");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="solutionService"></param>
        public SiteSettingsViewModel(ISolutionService solutionService) : base(Guid.NewGuid().ToString())
        {
            this.solutionService = solutionService;
        }

        #endregion

        #region Navigation Methods

        protected override void OnNavigatedTo(object parameter)
        {
            this.CurrentSiteSettings = this.solutionService.CurrentSolution.ProjectList.First().Site.SiteSettings.Clone() as SiteSettings; 
        }

        protected override void OnNavigatedFrom()
        {

        }

        #endregion

        #region ICustomOkCancelControl

        public object CustControlNavParams
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool CancelClicked()
        {
            return true;
        }

        public bool OkClicked()
        {
            if (this.CurrentSiteSettings == null)
                return false;

            this.solutionService.CurrentSolution.ProjectList.First().Site.SiteSettings.Overwrite(this.CurrentSiteSettings);

            return true;
        }

        #endregion
    }
}
