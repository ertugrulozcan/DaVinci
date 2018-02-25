using Ertis.DaVinci.Events;
using Ertis.DaVinci.HtmlModels;
using Ertis.DaVinci.Services.Interfaces;
using Ertis.Shared.ViewModels;
using Microsoft.Practices.Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.ViewModels
{
    public class WebBrowserViewModel : BaseViewModel
    {
        #region Services

        private readonly ISolutionService solutionService;
        private readonly IEventAggregator eventAggregator;

        #endregion

        #region Fields

        private Page selectedPage;

        #endregion

        #region Properties
        
        public Page SelectedPage
        {
            get
            {
                return selectedPage;
            }

            set
            {
                this.selectedPage = value;
                this.RaisePropertyChanged("SelectedPage");
                this.RaisePropertyChanged("SiteFilePath");
                this.RaisePropertyChanged("IsPageNull");
                this.RaisePropertyChanged("IsPageNotNull");
            }
        }

        public string SiteFilePath
        {
            get
            {
                if (this.solutionService == null || this.SelectedPage == null)
                    return null;

                string path = this.solutionService.TempFolderPath + this.SelectedPage.Name.Trim().ToLower() + ".html";
                if (System.IO.File.Exists(path))
                    return path;
                else
                    return null;
            }
        }

        public bool IsPageNull
        {
            get
            {
                return this.SelectedPage == null;
            }
        }

        public bool IsPageNotNull
        {
            get
            {
                return this.SelectedPage != null;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="guid"></param>
        public WebBrowserViewModel(ISolutionService solutionService, IEventAggregator eventAggregator) : base(Guid.NewGuid().ToString())
        {
            this.solutionService = solutionService;
            this.eventAggregator = eventAggregator;

            this.eventAggregator.GetEvent<SelectedPageChangedEvent>().Subscribe(OnSelectedPageChanged);
            this.eventAggregator.GetEvent<PageRefreshEvent>().Subscribe(OnPageRefreshEvent);
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

        #region Events

        private void OnSelectedPageChanged(Page page)
        {
            this.SelectedPage = page;
        }

        private void OnPageRefreshEvent(Page page)
        {
            if (this.SelectedPage == page)
            {
                this.RaisePropertyChanged("SelectedPage");
                this.RaisePropertyChanged("SiteFilePath");
                this.RaisePropertyChanged("IsPageNull");
                this.RaisePropertyChanged("IsPageNotNull");
            }
        }

        #endregion
    }
}
