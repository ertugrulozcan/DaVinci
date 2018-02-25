using Ertis.DaVinci.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.HtmlModels
{
    public class MainContent : IHtml
    {
        public List<Section> SectionList { get; set; }

        public ContactSection ContactSection { get; set; }

        public GoogleMapsSection GoogleMapsSection { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sectionList"></param>
        /// <param name="contactSection"></param>
        /// <param name="googleMapsSection"></param>
        public MainContent(List<Section> sectionList, ContactSection contactSection, GoogleMapsSection googleMapsSection)
        {
            this.SectionList = sectionList;
            this.ContactSection = contactSection;
            this.GoogleMapsSection = googleMapsSection;
        }

        public string GenerateCode()
        {
            string sectionsCode = string.Empty;

            foreach (var section in this.SectionList)
            {
                if (section is IHtml)
                    sectionsCode += (section as IHtml).GenerateCode() + Environment.NewLine;
            }

            string contactSectionCode = string.Empty;
            if (this.ContactSection != null)
                contactSectionCode = this.ContactSection.GenerateCode();

            string googleMapsSectionCode = string.Empty;
            if (this.GoogleMapsSection != null)
                googleMapsSectionCode = this.GoogleMapsSection.GenerateCode();

            return TemplateManager.Current.MainTemplate
                .Replace("{SECTIONS}", sectionsCode)
                .Replace("{CONTACT}", contactSectionCode)
                .Replace("{GOOGLE_MAP}", googleMapsSectionCode);
        }
    }
}
