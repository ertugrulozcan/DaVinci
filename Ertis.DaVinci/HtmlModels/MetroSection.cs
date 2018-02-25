using Ertis.DaVinci.Managers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.HtmlModels
{
    public class MetroSection : Section, IHtml
    {
        #region Fields

        private string subTitle;

        #endregion

        #region Properties

        public string SubTitle
        {
            get
            {
                return subTitle;
            }

            set
            {
                this.subTitle = value;
                this.RaisePropertyChanged("SubTitle");
            }
        }

        #endregion

        #region Constructors

        public MetroSection() : base(SectionType.MetroSection)
        {

        }

        #endregion

        #region Methods

        public string GenerateCode()
        {
            return TemplateManager.Current.MetroTemplate
                .Replace("{BACKGROUND_IMAGE_PATH}", this.BackgroundImagePath)
                .Replace("{TITLE}", this.Title)
                .Replace("{SUB_TITLE}", this.SubTitle);
        }

        #endregion
    }
}
