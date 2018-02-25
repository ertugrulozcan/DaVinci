using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.HtmlModels
{
    public class SubParagraph : HtmlModel
    {
        private string title;
        private string text;

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
            }
        }
    }
}
