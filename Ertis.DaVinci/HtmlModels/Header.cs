using Ertis.DaVinci.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.HtmlModels
{
    public class Header : IHtml
    {
        public string LogoPath { get; set; }

        public string GenerateCode()
        {
            return TemplateManager.Current.HeaderTemplate
                .Replace("{LOGO_PATH}", this.LogoPath);
        }
    }
}
