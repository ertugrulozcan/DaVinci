using Ertis.DaVinci.Managers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.HtmlModels
{
    public class BlogSection : Section, IHtml
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

        #endregion

        #region Constructors

        public BlogSection() : base(SectionType.BlogSection)
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
            string imagesCode = string.Empty;
            foreach (var imagePath in this.SliderImagePaths)
            {
                imagesCode += string.Format("<div class=\"item\"><img class=\"image\" src=\"{0}\"></div>", imagePath) + Environment.NewLine;
            }

            string textCode = string.Empty;
            var textParts = this.Text.Split('\n');

            foreach (var part in textParts)
            {
                if (part.StartsWith("•"))
                {
                    textCode += string.Format("<li>{0}</li>", part.Replace("•", string.Empty));
                }
                else
                {
                    textCode += string.Format("<p>{0}</p>", part);
                }
            }

            int liStartIndex = textCode.IndexOf("<li>");
            int liEndIndex = textCode.LastIndexOf("</li>");

            if (liStartIndex > 0 && liEndIndex > 0)
            {
                textCode = textCode.Insert(liStartIndex, "<ul class=\"list\">");
                textCode = textCode.Insert(liEndIndex + 22, "</ul>");
            }

            return TemplateManager.Current.BlogTemplate
                .Replace("{SUB_TITLE}", this.SubTitle)
                .Replace("{TEXT}", textCode)
                .Replace("{IMAGES}", imagesCode);
        }

        #endregion
    }
}
