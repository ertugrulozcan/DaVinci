using Ertis.DaVinci.Managers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.HtmlModels
{
    public class Page : HtmlModel, IHtml
    {
        #region Fields

        private string name;
        private string title;
        private ObservableCollection<Section> sectionList;
        private string logoPath;
        private bool hasContactSection = true;
        private bool hasGoogleMapsSection;

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

        public ObservableCollection<Section> SectionList
        {
            get
            {
                return sectionList;
            }

            set
            {
                this.sectionList = value;
                this.RaisePropertyChanged("SectionList");
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

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public Page()
        {
            this.SectionList = new ObservableCollection<Section>();
        }

        #endregion

        #region Methods

        public string GenerateCode()
        {
            Header header = new Header() { LogoPath = this.LogoPath };
            Footer footer = new Footer();

            ContactSection contactSection = null;
            if (this.HasContactSection)
                contactSection = new ContactSection();

            GoogleMapsSection googleMapsSection = null;
            if (this.HasGoogleMapsSection)
                googleMapsSection = new GoogleMapsSection();

            MainContent main = new MainContent(this.SectionList.ToList(), contactSection, googleMapsSection);
            Head head = new Head();
            Body body = new Body(header, main, footer);

            if (this.SectionList.Any(x => x.Type == SectionType.Gallery))
            {
                head.AddAdditionalCss("<link rel=\"stylesheet\" href=\"css/gallery.css\">");
            }

            return TemplateManager.Current.PageTemplate
                .Replace("{HEAD}", head.GenerateCode())
                .Replace("{BODY}", body.GenerateCode());
        }

        #endregion
    }
}
