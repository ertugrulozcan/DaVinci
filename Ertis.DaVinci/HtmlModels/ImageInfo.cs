using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.HtmlModels
{
    public class ImageInfo : HtmlModel
    {
        #region Fields

        private string title;
        private string subTitle;
        private string imagePath;

        #endregion

        #region Properties

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

        public string SubTitle
        {
            get
            {
                return subTitle;
            }

            set
            {
                this.subTitle = value;
                this.RaisePropertyChanged("SubTitle");
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

        #endregion
    }
}
