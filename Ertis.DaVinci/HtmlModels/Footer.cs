using Ertis.DaVinci.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.HtmlModels
{
    public class Footer : IHtml
    {
        public string GenerateCode()
        {
            var siteSettings = SolutionManager.Current.GetCurrentSiteSettings();
            string socialMediaIconsCode = string.Empty;

            if (siteSettings.HasFacebookAccount)
                socialMediaIconsCode += string.Format("<a href=\"{0}\" target=\"_blank\"><i class=\"ion ion-social-facebook size-18\"></i></a>", siteSettings.FacebookAccountLink);

            if (siteSettings.HasTwitterAccount)
                socialMediaIconsCode += string.Format("<a href=\"{0}\" target=\"_blank\"><i class=\"ion ion-social-twitter size-18\"></i></a>", siteSettings.TwitterAccountLink);

            if (siteSettings.HasInstagramAccount)
                socialMediaIconsCode += string.Format("<a href=\"{0}\" target=\"_blank\"><i class=\"ion ion-social-instagram size-18\"></i></a>", siteSettings.InstagramAccountLink);

            if (siteSettings.HasLinkedinAccount)
                socialMediaIconsCode += string.Format("<a href=\"{0}\" target=\"_blank\"><i class=\"ion ion-social-linkedin size-18\"></i></a>", siteSettings.LinkedinAccountLink);

            return TemplateManager.Current.FooterTemplate
                .Replace("{FOOTER_TEXT}", siteSettings.FooterText)
                .Replace("{SOCIAL_MEDIA_ICONS}", socialMediaIconsCode);
        }
    }
}
