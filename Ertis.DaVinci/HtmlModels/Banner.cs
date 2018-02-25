using Ertis.DaVinci.Managers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.HtmlModels
{
    public class Banner : Section, IHtml
    {
        #region Fields

        private string subTitle;
        private List<string> sliderImagePaths = new List<string>();

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

        public List<string> SliderImagePaths
        {
            get
            {
                return sliderImagePaths;
            }

            set
            {
                this.sliderImagePaths = value;
                this.RaisePropertyChanged("SliderImagePaths");
            }
        }

        [JsonIgnore]
        public string SliderImagePathsString
        {
            get
            {
                return string.Join("; ", this.SliderImagePaths);
            }

            private set
            {
                if (SolutionManager.Current.CurrentSolution == null)
                    return;

                this.SliderImagePaths.Clear();
                if (!(string.IsNullOrEmpty(value) || string.IsNullOrEmpty(value.Trim())))
                {
                    var splitList = value.Split(';').ToList();
                    if (splitList != null && splitList.Count > 0)
                    {
                        foreach (var path in splitList)
                        {
                            this.SliderImagePaths.Add(path.Trim());
                        }
                    }
                }
            }
        }

        [JsonIgnore]
        public string SectionCode
        {
            get
            {
                return this.GenerateCode();
            }
        }

        #endregion

        #region Constructors

        public Banner() : base(SectionType.BannerSection)
        {

        }

        #endregion

        #region Methods

        public void Raise()
        {
            this.RaisePropertyChanged("SliderImagePaths");
            this.RaisePropertyChanged("SliderImagePathsString");
        }

        public string GenerateCode()
        {
            string sliderPartsCode = string.Empty;
            int i = 1;
            foreach (var sliderImagePath in this.SliderImagePaths)
            {
                sliderPartsCode += TemplateManager.Current.BannerSlideTemplate
                    .Replace("{SLIDE_NO}", i.ToString())
                    .Replace("{IMAGE_PATH}", sliderImagePath)
                    .Replace("{TITLE}", this.Title)
                    .Replace("{SUB_TITLE}", this.SubTitle)
                    .Replace("{TEXT}", this.Text) + Environment.NewLine;
                i++;
            }

            return TemplateManager.Current.BannerTemplate.Replace("{SLIDER_PARTS}", sliderPartsCode);
        }

        #endregion
    }
}
