using Ertis.DaVinci.HtmlModels;
using Microsoft.Practices.Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.Events
{
    public class PageRefreshEvent : CompositePresentationEvent<Page>
    {
        public Page Page { get; set; }

        public PageRefreshEvent()
        {

        }

        public PageRefreshEvent(Page page)
        {
            this.Page = page;
        }
    }
}
