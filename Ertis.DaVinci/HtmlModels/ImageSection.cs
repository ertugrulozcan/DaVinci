using Ertis.DaVinci.Managers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.HtmlModels
{
    public class ImageSection : Section, IHtml
    {
        private ObservableCollection<ImageInfo> imageList = new ObservableCollection<ImageInfo>();

        public ObservableCollection<ImageInfo> ImageList
        {
            get
            {
                return imageList;
            }

            set
            {
                this.imageList = value;
                this.RaisePropertyChanged("ImageList");
            }
        }

        public ImageSection() : base(SectionType.ImageSection)
        {

        }

        public string GenerateCode()
        {
            string imageBoxesCode = string.Empty;

            foreach (var imageBox in this.ImageList)
            {
                imageBoxesCode += TemplateManager.Current.ImageBoxTemplate
                    .Replace("{IMAGE_PATH}", imageBox.ImagePath)
                    .Replace("{TITLE}", imageBox.Title)
                    .Replace("{SUB_TITLE}", imageBox.SubTitle) + Environment.NewLine;
            }

            return TemplateManager.Current.ImageSectionTemplate
                .Replace("{TITLE}", this.Title)
                .Replace("{TEXT}", this.Text)
                .Replace("{BACKGROUND_IMAGE_PATH}", this.BackgroundImagePath)
                .Replace("{IMAGE_BOXES}", imageBoxesCode);
        }
    }
}
