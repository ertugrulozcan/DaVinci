using Ertis.DaVinci.Events;
using Ertis.DaVinci.HtmlModels;
using Ertis.DaVinci.Models;
using Ertis.DaVinci.Services.Interfaces;
using Ertis.Shared.Models;
using Ertis.Shared.Services.Contracts;
using Ertis.Shared.ViewModels;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ertis.DaVinci.ViewModels
{
    public class DesignerViewModel : BaseViewModel
    {
        #region Services

        private readonly ISolutionService solutionService;
        private readonly IWindowNavigationService windowNavigationService;
        private readonly IEventAggregator eventAggregator;

        #endregion

        #region Fields

        private Project currentProject;
        private Page selectedPage;
        private Section selectedSection;
        private bool isEditMode;

        #endregion

        #region Properties

        public ModalViewMenuItem AddSectionVMI { get; private set; }

        public ModalViewMenuItem AddPageVMI { get; private set; }

        public ModalViewMenuItem SiteSettingsVMI { get; private set; }

        public Project CurrentProject
        {
            get
            {
                return currentProject;
            }

            set
            {
                this.currentProject = value;
                this.RaisePropertyChanged("CurrentProject");
            }
        }

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

                this.eventAggregator.GetEvent<SelectedPageChangedEvent>().Publish(value);
            }
        }

        public Section SelectedSection
        {
            get
            {
                return selectedSection;
            }

            set
            {
                this.selectedSection = value;
                this.RaisePropertyChanged("SelectedSection");
            }
        }

        public bool IsEditMode
        {
            get
            {
                return isEditMode;
            }

            set
            {
                this.isEditMode = value;
                this.RaisePropertyChanged("IsEditMode");
            }
        }
        
        #endregion

        #region Commands

        public DelegateCommand AddSectionButtonCommand { get; set; }

        public DelegateCommand AddNewPageCommand { get; set; }

        public DelegateCommand<Section> EditSectionCommand { get; set; }

        public DelegateCommand<Section> DeleteSectionCommand { get; set; }

        public DelegateCommand<Section> MoveUpSectionCommand { get; set; }

        public DelegateCommand<Section> MoveDownSectionCommand { get; set; }

        public DelegateCommand PageSettingsButtonCommand { get; set; }

        public DelegateCommand SiteSettingsButtonCommand { get; set; }
        
        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="guid"></param>
        public DesignerViewModel(ISolutionService solutionService, IWindowNavigationService windowNavigationService, IEventAggregator eventAggregator) : base(Guid.NewGuid().ToString())
        {
            this.solutionService = solutionService;
            this.windowNavigationService = windowNavigationService;
            this.eventAggregator = eventAggregator;

            this.AddSectionVMI = new ModalViewMenuItem(102, typeof(Views.AddSectionView), "NewSection", "List");
            this.AddPageVMI = new ModalViewMenuItem(103, typeof(Views.AddPageView), "NewPage", "Clone");
            this.SiteSettingsVMI = new ModalViewMenuItem(104, typeof(Views.SiteSettingsView), "SiteSettings", "Clone");

            this.AddSectionButtonCommand = new DelegateCommand(ExecuteAddSectionButtonCommand, CanExecuteAddSectionButtonCommand);
            this.AddNewPageCommand = new DelegateCommand(ExecuteAddNewPageCommand, CanExecuteAddNewPageCommand);
            this.EditSectionCommand = new DelegateCommand<Section>(ExecuteEditSectionCommand, CanExecuteEditSectionCommand);
            this.DeleteSectionCommand = new DelegateCommand<Section>(ExecuteDeleteSectionCommand, CanExecuteDeleteSectionCommand);
            this.MoveUpSectionCommand = new DelegateCommand<Section>(ExecuteMoveUpSectionCommand, CanExecuteMoveUpSectionCommand);
            this.MoveDownSectionCommand = new DelegateCommand<Section>(ExecuteMoveDownSectionCommand, CanExecuteMoveDownSectionCommand);
            this.PageSettingsButtonCommand = new DelegateCommand(ExecutePageSettingsButtonCommand, CanExecutePageSettingsButtonCommand);
            this.SiteSettingsButtonCommand = new DelegateCommand(ExecuteSiteSettingsButtonCommand, CanExecuteSiteSettingsButtonCommand);

            this.CurrentProject = this.solutionService.CurrentSolution.ProjectList.FirstOrDefault();

            this.eventAggregator.GetEvent<Events.PageAddedEvent>().Subscribe(OnPageAdded);
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

        #region Event Handlers

        private void OnPageAdded(Page page)
        {
            this.SelectedPage = page;

            this.solutionService.RefreshWebSite();
        }

        #endregion

        #region Command Methods

        private void ExecuteAddSectionButtonCommand()
        {
            if (this.SelectedPage != null)
                this.windowNavigationService.NavigateView(this.AddSectionVMI, this.SelectedPage);
        }

        private bool CanExecuteAddSectionButtonCommand()
        {
            return true;
        }

        private void ExecuteAddNewPageCommand()
        {
            this.windowNavigationService.NavigateView(this.AddPageVMI, this.CurrentProject);
        }

        private bool CanExecuteAddNewPageCommand()
        {
            return true;
        }

        private void ExecuteEditSectionCommand(Section section)
        {
            if (section == null)
                return;

            this.windowNavigationService.NavigateView(this.AddSectionVMI, section);
        }

        private bool CanExecuteEditSectionCommand(Section section)
        {
            return true;
        }

        private void ExecuteDeleteSectionCommand(Section section)
        {
            if (section == null)
                return;

            DialogResult result = System.Windows.Forms.MessageBox.Show(Localization.LocalizationUtility.Convert("DeleteSectionMessage"), Localization.LocalizationUtility.Convert("AreYouSure"), MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                this.SelectedPage.SectionList.Remove(section);
            }
        }

        private bool CanExecuteDeleteSectionCommand(Section section)
        {
            return true;
        }

        private void ExecuteMoveUpSectionCommand(Section section)
        {
            if (section == null)
                return;

            int index = this.SelectedPage.SectionList.IndexOf(section);
            if (index > 0)
            {
                this.SelectedPage.SectionList.Remove(section);
                this.SelectedPage.SectionList.Insert(index - 1, section);
            }
        }

        private bool CanExecuteMoveUpSectionCommand(Section section)
        {
            return true;
        }

        private void ExecuteMoveDownSectionCommand(Section section)
        {
            if (section == null)
                return;

            var index = this.SelectedPage.SectionList.IndexOf(section);
            if (index != -1 && index + 1 < this.SelectedPage.SectionList.Count)
            {
                this.SelectedPage.SectionList.Remove(section);
                this.SelectedPage.SectionList.Insert(index + 1, section);
            }
        }

        private bool CanExecuteMoveDownSectionCommand(Section section)
        {
            return true;
        }

        private void ExecutePageSettingsButtonCommand()
        {
            if (this.SelectedPage != null)
                this.windowNavigationService.NavigateView(this.AddPageVMI, this.SelectedPage);
        }

        private bool CanExecutePageSettingsButtonCommand()
        {
            return true;
        }

        private void ExecuteSiteSettingsButtonCommand()
        {
            this.windowNavigationService.NavigateView(this.SiteSettingsVMI, null);
        }

        private bool CanExecuteSiteSettingsButtonCommand()
        {
            return true;
        }

        #endregion
    }
}
