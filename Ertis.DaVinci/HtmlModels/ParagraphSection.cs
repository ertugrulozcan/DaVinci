using Ertis.DaVinci.Managers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.HtmlModels
{
    public class ParagraphSection : Section, IHtml
    {
        private ObservableCollection<SubParagraph> subParagraphList = new ObservableCollection<SubParagraph>();

        public ObservableCollection<SubParagraph> SubParagraphList
        {
            get
            {
                return subParagraphList;
            }

            set
            {
                this.subParagraphList = value;
                this.RaisePropertyChanged("SubParagraphList");
            }
        }

        public ParagraphSection() : base(SectionType.ParagraphSection)
        {

        }

        public string GenerateCode()
        {
            string paragraphsCode = string.Empty;

            var paragraphs = this.Text.Split('\n');
            foreach (var paragraph in paragraphs)
            {
                paragraphsCode += string.Format("<p class=\"principal text-white indented\">{0}</p>", paragraph) + Environment.NewLine;
            }

            string subParagraphsCode = string.Empty;
            foreach (var subParagraph in this.SubParagraphList)
            {
                subParagraphsCode += string.Format("<div class=\"item\"><h5>{0}</h5><p class=\"text-white indented\">{1}</p></div>", subParagraph.Title, subParagraph.Text) + Environment.NewLine;
            }

            if (!string.IsNullOrEmpty(subParagraphsCode))
            {
                subParagraphsCode = string.Format("<div class=\"features m-t-5\">{0}</div>", subParagraphsCode);
            }
            
            return TemplateManager.Current.ParagraphSectionTemplate
                .Replace("{TITLE}", this.Title)
                .Replace("{PARAGRAPHS}", paragraphsCode)
                .Replace("{SUB_PARAGRAPHS}", subParagraphsCode)
                .Replace("{BACKGROUND_IMAGE_PATH}", this.BackgroundImagePath);
        }
    }
}
