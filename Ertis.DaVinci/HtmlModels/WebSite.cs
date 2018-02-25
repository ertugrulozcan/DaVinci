using Ertis.DaVinci.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.HtmlModels
{
    public class WebSite : HtmlModel
    {
        #region Fields

        private string name;
        private string title;
        private string iconPath;
        private string logoPath;
        private ObservableCollection<Page> pageList;
        private SiteSettings siteSettings;

        #endregion

        #region Properties

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

        public string IconPath
        {
            get
            {
                return iconPath;
            }

            set
            {
                this.iconPath = value;
                this.RaisePropertyChanged("IconPath");
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

        public ObservableCollection<Page> PageList
        {
            get
            {
                return pageList;
            }

            set
            {
                this.pageList = value;
                this.RaisePropertyChanged("PageList");
            }
        }

        public SiteSettings SiteSettings
        {
            get
            {
                return siteSettings;
            }

            set
            {
                this.siteSettings = value;
                this.RaisePropertyChanged("SiteSettings");
            }
        }

        #endregion

        #region Constructors

        public WebSite()
        {
            this.PageList = new ObservableCollection<Page>();
        }

        #endregion
    }
}
