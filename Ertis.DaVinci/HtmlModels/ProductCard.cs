using Ertis.DaVinci.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.HtmlModels
{
    public class ProductCard : HtmlModel, IHtml
    {
        private string title;
        private string text;
        private string imagePath;
        private string link;

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

        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                this.text = value;
                this.RaisePropertyChanged("Text");
            }
        }

        public string ImagePath
        {
            get
            {
                return imagePath;
            }

            set
            {
                this.imagePath = value;
                this.RaisePropertyChanged("ImagePath");
            }
        }

        public string Link
        {
            get
            {
                return link;
            }

            set
            {
                this.link = value;
                this.RaisePropertyChanged("Link");
            }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public ProductCard()
        {

        }

        /// <summary>
        /// Constructors
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="imagePath"></param>
        /// <param name="link"></param>
        public ProductCard(string title, string text, string imagePath, string link)
        {
            this.Title = title;
            this.Text = text;
            this.ImagePath = imagePath;
            this.Link = link;
        }

        public string GenerateCode()
        {
            return TemplateManager.Current.ProductCardTemplate
                .Replace("{TITLE}", this.Title)
                .Replace("{TEXT}", this.Text)
                .Replace("{IMAGE_PATH}", this.ImagePath)
                .Replace("{LINK}", this.Link);
        }
    }
}
