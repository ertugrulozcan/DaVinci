using Ertis.DaVinci.HtmlModels;
using Ertis.DaVinci.Models;
using Ertis.DaVinci.Services.Interfaces;
using Ertis.Shared.ModalWindow.Contracts;
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
    public class AddPageViewModel : BaseViewModel, ICustomOkCancelControl
    {
        #region Services

        private readonly ISolutionService solutionService;
        private readonly IWindowNavigationService windowNavigationService;
        private readonly IEventAggregator eventAggregator;

        #endregion

        #region Fields

        private string name;
        private string title;
        private string logoPath;
        private bool hasContactSection = true;
        private bool hasGoogleMapsSection;
        private bool isEditMode;

        #endregion

        #region Properties

        public Project CurrentProject { get; private set; }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                this.name = value;
                this.RaisePropertyChanged("Name");
                this.RaisePropertyChanged("PagePath");
            }
        }

        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                this.title = value;
                this.RaisePropertyChanged("Title");
            }
        }

        public string LogoPath
        {
            get
            {
                return logoPath;
            }

            set
            {
                this.logoPath = value;
                this.RaisePropertyChanged("LogoPath");
            }
        }

        public string PagePath
        {
            get
            {
                if (this.CurrentProject == null || this.Name == null)
                    return string.Empty;

                return string.Format("{0}\\{1}.html", this.solutionService.SolutionPath, this.Name.Trim().ToLower());
            }
        }

        public bool HasContactSection
        {
            get
            {
                return hasContactSection;
            }

            set
            {
                this.hasContactSection = value;
                this.RaisePropertyChanged("HasContactSection");
            }
        }

        public bool HasGoogleMapsSection
        {
            get
            {
                return hasGoogleMapsSection;
            }

            set
            {
                this.hasGoogleMapsSection = value;
                this.RaisePropertyChanged("HasGoogleMapsSection");
            }
        }

        public Page EditingPage { get; private set; }

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
                this.RaisePropertyChanged("IsNotEditMode");
            }
        }

        public bool IsNotEditMode
        {
            get
            {
                return !this.IsEditMode;
            }
        }

        #endregion

        #region Commands

        public DelegateCommand SelectLogoButtonCommand { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="solutionService"></param>
        public AddPageViewModel(ISolutionService solutionService, IWindowNavigationService windowNavigationService, IEventAggregator eventAggregator) : base(Guid.NewGuid().ToString())
        {
            this.solutionService = solutionService;
            this.windowNavigationService = windowNavigationService;
            this.eventAggregator = eventAggregator;

            this.SelectLogoButtonCommand = new DelegateCommand(ExecuteSelectLogoButtonCommand, CanExecuteSelectLogoButtonCommand);
        }

        #endregion

        #region Navigate Methods

        protected override void OnNavigatedFrom()
        {

        }

        protected override void OnNavigatedTo(object parameter)
        {
            var navigationParameter = this.windowNavigationService.GetNavigationParameter(typeof(Views.AddPageView).FullName);

            if (navigationParameter is Project)
            {
                this.IsEditMode = false;
                this.CurrentProject = navigationParameter as Project;
            }
            else if (navigationParameter is Page)
            {
                this.EditingPage = navigationParameter as Page;
                this.IsEditMode = true;
                this.Name = this.EditingPage.Name;
                this.Title = this.EditingPage.Title;
                this.LogoPath = this.EditingPage.LogoPath;
                this.HasContactSection = this.EditingPage.HasContactSection;
                this.HasGoogleMapsSection = this.EditingPage.HasGoogleMapsSection;
            }
        }

        #endregion

        #region Commands

        private void ExecuteSelectLogoButtonCommand()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*";
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedDirectoryPath = this.solutionService.SolutionPath;
                string fileName = openFileDialog.FileName;

                System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileName);
                if (fileInfo.DirectoryName != selectedDirectoryPath + "\\img")
                {
                    if (System.IO.File.Exists(selectedDirectoryPath + "\\img\\" + fileInfo.Name))
                    {
                        DialogResult result = System.Windows.Forms.MessageBox.Show("Images klasöründe aynı isimde bir dosya var. Değiştirilsin mi?", "Select Image", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            System.IO.File.Copy(fileName, selectedDirectoryPath + "\\img\\" + fileInfo.Name, true);
                        }
                    }
                    else
                    {
                        System.IO.File.Copy(fileName, selectedDirectoryPath + "\\img\\" + fileInfo.Name);
                    }
                }

                this.LogoPath = "img/" + fileInfo.Name;
            }
        }

        private bool CanExecuteSelectLogoButtonCommand()
        {
            return true;
        }

        #endregion

        #region ICustomOkCancelControl

        public bool OkClicked()
        {
            if (this.IsEditMode && this.EditingPage != null)
            {
                this.EditingPage.Name = this.Name;
                this.EditingPage.Title = this.Title;
                this.EditingPage.LogoPath = this.LogoPath;
                this.EditingPage.HasContactSection = this.HasContactSection;
                this.EditingPage.HasGoogleMapsSection = this.HasGoogleMapsSection;
            }
            else
            {
                var page = new Page()
                {
                    Name = this.Name,
                    Title = this.Title,
                    LogoPath = this.LogoPath,
                    HasContactSection = this.HasContactSection,
                    HasGoogleMapsSection = this.HasGoogleMapsSection
            };

                this.CurrentProject.Site.PageList.Add(page);

                this.eventAggregator.GetEvent<Events.PageAddedEvent>().Publish(page);
            }

            return true;
        }

        public bool CancelClicked()
        {
            return true;
        }

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

        #endregion
    }
}
