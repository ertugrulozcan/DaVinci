using Ertis.DaVinci.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.HtmlModels
{
    public class Body : IHtml
    {
        public Header Header { get; set; }

        public MainContent Main { get; set; }

        public Footer Footer { get; set; }

        /// <summary>
        /// Constructors
        /// </summary>
        /// <param name="header"></param>
        /// <param name="main"></param>
        /// <param name="footer"></param>
        public Body(Header header, MainContent main, Footer footer)
        {
            this.Header = header;
            this.Main = main;
            this.Footer = footer;
        }

        public string GenerateCode()
        {
            string blankSectionCode = string.Empty;
            if (!this.Main.SectionList.Any(x => x.Type == SectionType.BannerSection || x.Type == SectionType.MetroSection))
            {
                blankSectionCode = "<section class=\"section-main bg-two\"><div class=\"container py-6\"></div></section>";
            }

            return TemplateManager.Current.BodyTemplate
                .Replace("{BLANK_SECTION}", blankSectionCode)
                .Replace("{HEADER}", this.Header.GenerateCode())
                .Replace("{MAIN}", this.Main.GenerateCode())
                .Replace("{FOOTER}", this.Footer.GenerateCode());
        }
    }
}
