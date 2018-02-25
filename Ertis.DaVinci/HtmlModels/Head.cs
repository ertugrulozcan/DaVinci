using Ertis.DaVinci.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.HtmlModels
{
    public class Head : IHtml
    {
        private List<string> additionalCssList = new List<string>();

        public string GenerateCode()
        {
            string additionalCssCode = string.Empty;
            foreach (var css in this.additionalCssList)
            {
                additionalCssCode += css + Environment.NewLine;
            }

            return TemplateManager.Current.HeadTemplate
                .Replace("{ADDITIONAL_CSS}", additionalCssCode);
        }

        internal void AddAdditionalCss(string css)
        {
            this.additionalCssList.Add(css);
        }
    }
}
