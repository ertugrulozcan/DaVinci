using Ertis.DaVinci.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.HtmlModels
{
    public class GoogleMapsSection : Section, IHtml
    {
        public GoogleMapsSection() : base(SectionType.GoogleMapsSection)
        {

        }

        public string GenerateCode()
        {
            var siteSettings = SolutionManager.Current.GetCurrentSiteSettings();
            return TemplateManager.Current.GoogleMapTemplate
                .Replace("{LATITUDE}", siteSettings.Latitude)
                .Replace("{LONGITUDE}", siteSettings.Longitude);
        }
    }
}
