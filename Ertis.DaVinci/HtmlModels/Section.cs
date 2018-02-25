using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.HtmlModels
{
    public abstract class Section : HtmlModel
    {
        #region Fields

        private SectionType type;
        private string title;
        private string text;
        private string backgroundImagePath;
        private double marginTop;
        private double marginLeft;
        private double marginRight;
        private double marginBottom;

        #endregion

        #region Properties

        public SectionType Type
        {
            get
            {
                return type;
            }

            set
            {
                this.type = value;
                this.RaisePropertyChanged("Type");
            }
        }

        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                this.title = value;
                this.RaisePropertyChanged("Title");
                this.RaisePropertyChanged("HasTitle");
            }
        }

        [JsonIgnore]
        public bool HasTitle
        {
            get
            {
                return !string.IsNullOrEmpty(this.Title);
            }
        }

        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                this.text = value;
                this.RaisePropertyChanged("Text");
                this.RaisePropertyChanged("HasText");
            }
        }

        [JsonIgnore]
        public bool HasText
        {
            get
            {
                return !string.IsNullOrEmpty(this.Text);
            }
        }

        public string BackgroundImagePath
        {
            get
            {
                return backgroundImagePath;
            }

            set
            {
                this.backgroundImagePath = value;
                this.RaisePropertyChanged("BackgroundImagePath");
                this.RaisePropertyChanged("HasBackgroundImage");
            }
        }

        [JsonIgnore]
        public bool HasBackgroundImage
        {
            get
            {
                return !string.IsNullOrEmpty(this.BackgroundImagePath);
            }
        }

        public double MarginTop
        {
            get
            {
                return marginTop;
            }

            set
            {
                this.marginTop = value;
                this.RaisePropertyChanged("MarginTop");
            }
        }

        public double MarginLeft
        {
            get
            {
                return marginLeft;
            }

            set
            {
                this.marginLeft = value;
                this.RaisePropertyChanged("MarginLeft");
            }
        }

        public double MarginRight
        {
            get
            {
                return marginRight;
            }

            set
            {
                this.marginRight = value;
                this.RaisePropertyChanged("MarginRight");
            }
        }

        public double MarginBottom
        {
            get
            {
                return marginBottom;
            }

            set
            {
                this.marginBottom = value;
                this.RaisePropertyChanged("MarginBottom");
            }
        }

        #endregion

        #region Constructors
        
        public Section(SectionType type)
        {
            this.Type = type;
        }

        #endregion
    }
}
