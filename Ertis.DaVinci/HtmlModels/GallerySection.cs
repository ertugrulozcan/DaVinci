using Ertis.DaVinci.Managers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.HtmlModels
{
    public class GallerySection : Section, IHtml
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

        public GallerySection() : base(SectionType.Gallery)
        {

        }

        public string GenerateCode()
        {
            string imageBoxesCode = string.Empty;

            foreach (var imageBox in this.ImageList)
            {
                imageBoxesCode += string.Format("<a href=\"{0}\" data-rel=\"lightcase:gallery\" title=\"{1}\"><img src=\"{0}\" alt=\"\"></a>", imageBox.ImagePath, imageBox.Title) + Environment.NewLine;
            }

            return TemplateManager.Current.GalleryTemplate
                .Replace("{TITLE}", this.Title)
                .Replace("{IMAGE_BOXES}", imageBoxesCode) + Environment.NewLine;
        }
    }
}
