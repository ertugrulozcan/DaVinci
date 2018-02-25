using Ertis.DaVinci.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.HtmlModels
{
    public class BasicSection : Section, IHtml
    {
        #region Constructors

        public BasicSection() : base(SectionType.BasicSection)
        {

        }

        #endregion

        #region Methods

        public string GenerateCode()
        {
            string imageCode = string.Empty;
            if (this.HasBackgroundImage)
                imageCode = string.Format("<img src=\"{0}\" alt=\"\">", this.BackgroundImagePath);

            return TemplateManager.Current.BasicSectionTemplate
                .Replace("{IMAGE_CODE}", imageCode)
                .Replace("{TITLE}", this.Title)
                .Replace("{TEXT}", this.Text);
        }

        #endregion
    }
}
