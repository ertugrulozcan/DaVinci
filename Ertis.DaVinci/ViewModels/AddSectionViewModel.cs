using Ertis.DaVinci.HtmlModels;
using Ertis.DaVinci.Services.Interfaces;
using Ertis.Shared.ModalWindow.Contracts;
using Ertis.Shared.Services.Contracts;
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
    public class AddSectionViewModel : BaseViewModel, ICustomOkCancelControl
    {
        #region Services

        private readonly ISolutionService solutionService;
        private readonly IWindowNavigationService windowNavigationService;

        #endregion

        #region Fields

        private Section selectedSection;
        private ProductCard selectedCard;
        private bool isEditMode;

        #endregion

        #region Properties

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

        public ProductCard SelectedCard
        {
            get
            {
                return selectedCard;
            }

            set
            {
                this.selectedCard = value;
                this.RaisePropertyChanged("SelectedCard");
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

        public Banner BannerSection { get; private set; }

        public BasicSection BasicSection { get; private set; }

        public ParagraphSection ParagraphSection { get; private set; }

        public ImageSection ImageSection { get; private set; }

        public CardsSection CardsSection { get; private set; }

        public MetroSection MetroSection { get; private set; }

        public BlogSection BlogSection { get; private set; }

        public GallerySection GallerySection { get; private set; }

        #endregion

        #region Commands

        public DelegateCommand<Section> SelectBackgroundImageButtonCommand { get; set; }

        public DelegateCommand<ParagraphSection> AddSubParagraphCommand { get; set; }

        public DelegateCommand<ImageSection> AddImageInfoCommand { get; set; }

        public DelegateCommand<ImageInfo> SelectImageInfoButtonCommand { get; set; }

        public DelegateCommand<ImageInfo> ImageInfoRemoveCommand { get; set; }

        public DelegateCommand<CardsSection> AddCardButtonCommand { get; set; }

        public DelegateCommand<SubParagraph> SubParagraphRemoveCommand { get; set; }
        
        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="solutionService"></param>
        /// <param name="windowNavigationService"></param>
        public AddSectionViewModel(ISolutionService solutionService, IWindowNavigationService windowNavigationService) : base(Guid.NewGuid().ToString())
        {
            this.solutionService = solutionService;
            this.windowNavigationService = windowNavigationService;

            this.BannerSection = new Banner();
            this.MetroSection = new MetroSection();
            this.BasicSection = new BasicSection();
            this.ParagraphSection = new ParagraphSection();
            this.ImageSection = new ImageSection();
            this.CardsSection = new CardsSection();
            this.BlogSection = new BlogSection();
            this.GallerySection = new GallerySection();

            this.SelectBackgroundImageButtonCommand = new DelegateCommand<Section>(ExecuteSelectBackgroundImageButtonCommand, CanExecuteSelectBackgroundImageButtonCommand);
            this.AddSubParagraphCommand = new DelegateCommand<ParagraphSection>(ExecuteAddSubParagraphCommand, CanExecuteAddSubParagraphCommand);
            this.AddImageInfoCommand = new DelegateCommand<ImageSection>(ExecuteAddImageInfoCommand, CanExecuteAddImageInfoCommand);
            this.SelectImageInfoButtonCommand = new DelegateCommand<ImageInfo>(ExecuteSelectImageInfoButtonCommand, CanExecuteSelectImageInfoButtonCommand);
            this.ImageInfoRemoveCommand = new DelegateCommand<ImageInfo>(ExecuteImageInfoRemoveCommand, CanExecuteImageInfoRemoveCommand);
            this.AddCardButtonCommand = new DelegateCommand<CardsSection>(ExecuteAddCardButtonCommand, CanExecuteAddCardButtonCommand);
            this.SubParagraphRemoveCommand = new DelegateCommand<SubParagraph>(ExecuteSubParagraphRemoveCommand, CanExecuteSubParagraphRemoveCommand);
        }

        #endregion

        #region Navigate Methods

        protected override void OnNavigatedFrom()
        {

        }

        protected override void OnNavigatedTo(object parameter)
        {
            var navigationParameter = this.windowNavigationService.GetNavigationParameter(typeof(Views.AddSectionView).FullName);
            if (navigationParameter is Section)
            {
                this.IsEditMode = true;
                this.SelectedSection = navigationParameter as Section;
            }
        }

        #endregion

        #region ICustomOkCancelControl

        public bool OkClicked()
        {
            if (this.IsEditMode)
            {

            }
            else
            {
                var currentPage = this.windowNavigationService.GetNavigationParameter(typeof(Views.AddSectionView).FullName) as Page;
                currentPage.SectionList.Add(this.SelectedSection);
            }

            this.solutionService.RefreshWebSite();

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

        #region Command Methods

        private void ExecuteSelectBackgroundImageButtonCommand(Section section)
        {
            if (section == null)
            {
                return;
            }

            string selectedDirectoryPath = this.solutionService.SolutionPath;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = selectedDirectoryPath + "\\img";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (openFileDialog.Multiselect)
                {
                    foreach (var fileName in openFileDialog.FileNames)
                    {
                        this.PerformSelectImageOperation(fileName, selectedDirectoryPath, section);
                    }
                }
                else
                {
                    this.PerformSelectImageOperation(openFileDialog.FileName, selectedDirectoryPath, section);
                }
            }
        }

        private void PerformSelectImageOperation(string fileName, string selectedDirectoryPath, Section section)
        {
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

            if (section is Banner)
            {
                (section as Banner).SliderImagePaths.Add("img/" + fileInfo.Name);
                (section as Banner).Raise();
            }
            if (section is BlogSection)
            {
                (section as BlogSection).SliderImagePaths.Add("img/" + fileInfo.Name);
                (section as BlogSection).Raise();
            }
            else if (section is CardsSection)
            {
                if (this.SelectedCard != null)
                    this.SelectedCard.ImagePath = "img/" + fileInfo.Name;
            }
            else if (section is GallerySection)
            {
                var gallery = section as GallerySection;
                gallery.ImageList.Add(new ImageInfo()
                {
                    ImagePath = "img/" + fileInfo.Name
                });
            }
            else
            {
                section.BackgroundImagePath = "img/" + fileInfo.Name;
            }
        }

        private bool CanExecuteSelectBackgroundImageButtonCommand(Section section)
        {
            return true;
        }

        private void ExecuteAddSubParagraphCommand(ParagraphSection section)
        {
            if (section != null)
                section.SubParagraphList.Add(new SubParagraph());
        }

        private bool CanExecuteAddSubParagraphCommand(ParagraphSection section)
        {
            return true;
        }

        private void ExecuteAddImageInfoCommand(ImageSection section)
        {
            if (section != null)
                section.ImageList.Add(new ImageInfo());
        }

        private bool CanExecuteAddImageInfoCommand(ImageSection arg)
        {
            return true;
        }

        private void ExecuteSelectImageInfoButtonCommand(ImageInfo imageInfo)
        {
            if (imageInfo != null)
            {
                string selectedDirectoryPath = this.solutionService.SolutionPath;

                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*";
                openFileDialog.InitialDirectory = selectedDirectoryPath + "\\img";
                openFileDialog.Multiselect = false;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(openFileDialog.FileName);
                    if (fileInfo.DirectoryName != selectedDirectoryPath + "\\img")
                    {
                        if (System.IO.File.Exists(selectedDirectoryPath + "\\img\\" + fileInfo.Name))
                        {
                            DialogResult result = System.Windows.Forms.MessageBox.Show("Images klasöründe aynı isimde bir dosya var. Değiştirilsin mi?", "Select Image", MessageBoxButtons.YesNo);
                            if (result == DialogResult.Yes)
                            {
                                System.IO.File.Copy(openFileDialog.FileName, selectedDirectoryPath + "\\img\\" + fileInfo.Name, true);
                            }
                        }
                        else
                        {
                            System.IO.File.Copy(openFileDialog.FileName, selectedDirectoryPath + "\\img\\" + fileInfo.Name);
                        }
                    }

                    imageInfo.ImagePath = "img/" + fileInfo.Name;
                }
            }
        }

        private bool CanExecuteSelectImageInfoButtonCommand(ImageInfo arg)
        {
            return true;
        }

        private void ExecuteImageInfoRemoveCommand(ImageInfo imageInfo)
        {
            if (this.SelectedSection is ImageSection)
            {
                int removeIndex = (this.SelectedSection as ImageSection).ImageList.IndexOf(imageInfo);
                if (removeIndex >= 0)
                    (this.SelectedSection as ImageSection).ImageList.RemoveAt(removeIndex);
            }

            if (this.SelectedSection is GallerySection)
            {
                int removeIndex = (this.SelectedSection as GallerySection).ImageList.IndexOf(imageInfo);
                if (removeIndex >= 0)
                    (this.SelectedSection as GallerySection).ImageList.RemoveAt(removeIndex);
            }
        }

        private bool CanExecuteImageInfoRemoveCommand(ImageInfo arg)
        {
            return true;
        }

        private void ExecuteAddCardButtonCommand(CardsSection section)
        {
            if (section != null)
            {
                var card = new ProductCard();
                card.Title = "Card " + (section.Cards.Count + 1);
                section.Cards.Add(card);
                this.SelectedCard = card;
            }   
        }

        private bool CanExecuteAddCardButtonCommand(CardsSection arg)
        {
            return true;
        }

        private void ExecuteSubParagraphRemoveCommand(SubParagraph subParagraph)
        {
            if (this.SelectedSection is ParagraphSection)
            {
                int removeIndex = (this.SelectedSection as ParagraphSection).SubParagraphList.IndexOf(subParagraph);
                if (removeIndex >= 0)
                    (this.SelectedSection as ParagraphSection).SubParagraphList.RemoveAt(removeIndex);
            }
        }

        private bool CanExecuteSubParagraphRemoveCommand(SubParagraph arg)
        {
            return true;
        }

        #endregion
    }
}
