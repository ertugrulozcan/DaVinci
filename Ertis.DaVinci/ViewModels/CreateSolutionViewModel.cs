using Ertis.DaVinci.Services.Interfaces;
using Ertis.Shared.Components;
using Ertis.Shared.ModalWindow.Contracts;
using Ertis.Shared.ViewModels;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ertis.DaVinci.ViewModels
{
    public class CreateSolutionViewModel : BaseViewModel, ICustomOkCancelControl
    {
        #region Services

        private readonly ISolutionService solutionService;

        #endregion

        #region Fields

        private string projectName;
        private string folderPath;

        #endregion

        #region Properties

        public string ProjectName
        {
            get
            {
                return projectName;
            }

            set
            {
                this.projectName = value;
                this.RaisePropertyChanged("ProjectName");
            }
        }

        public string FolderPath
        {
            get
            {
                return folderPath;
            }

            set
            {
                this.folderPath = value;
                this.RaisePropertyChanged("FolderPath");
            }
        }

        #endregion

        #region Commands

        public DelegateCommand SelectFolderButtonCommand { get; set; }
        
        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="guid"></param>
        public CreateSolutionViewModel(ISolutionService solutionService) : base(Guid.NewGuid().ToString())
        {
            this.solutionService = solutionService;

            this.SelectFolderButtonCommand = new DelegateCommand(ExecuteSelectFolderButtonCommand, CanExecuteSelectFolderButtonCommand);
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

        #region Methods

        private bool IsValid()
        {
            if (string.IsNullOrEmpty(this.ProjectName))
            {
                CustomMessageBox.Show("Error", "You must enter a valid project name!", CustomMessageBox.MessageTypes.Warning, CustomMessageBox.DialogTypes.Ok);
                return false;
            }

            
            if (string.IsNullOrEmpty(this.FolderPath) || !System.IO.Directory.Exists(this.FolderPath))
            {
                CustomMessageBox.Show("Error", "You must select a path for project!", CustomMessageBox.MessageTypes.Warning, CustomMessageBox.DialogTypes.Ok);
                return false;
            }

            return true;
        }

        #endregion

        #region Command Methods

        private void ExecuteSelectFolderButtonCommand()
        {
            var dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.FolderPath = dialog.SelectedPath;
            }
        }

        private bool CanExecuteSelectFolderButtonCommand()
        {
            return true;
        }

        #endregion

        #region ICustomOkCancelControl

        public bool OkClicked()
        {
            bool isValid = this.IsValid();

            if (isValid)
            {
                this.solutionService.CreateSolution(this.ProjectName, this.FolderPath);
            }

            return isValid;
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
