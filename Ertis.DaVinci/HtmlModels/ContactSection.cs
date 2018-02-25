using Ertis.DaVinci.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.HtmlModels
{
    public class ContactSection : Section, IHtml
    {
        public ContactSection() : base(SectionType.ContactSection)
        {

        }

        public string GenerateCode()
        {
            var siteSettings = SolutionManager.Current.GetCurrentSiteSettings();
            var addressParts = siteSettings.Address.Split('\n');
            string addressCode = string.Empty;
            foreach (var part in addressParts)
            {
                addressCode += string.Format("<li>{0}</li>", part);
            }

            return TemplateManager.Current.ContactTemplate
                .Replace("{EMAIL_ADDRESS}", siteSettings.EmailAddress)
                .Replace("{PHONE_NUMBER}", siteSettings.PhoneNumber)
                .Replace("{ADDRESS}", addressCode);
        }
    }
}
