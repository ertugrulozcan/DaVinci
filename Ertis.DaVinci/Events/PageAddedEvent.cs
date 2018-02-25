using Ertis.DaVinci.HtmlModels;
using Microsoft.Practices.Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.Events
{
    public class PageAddedEvent : CompositePresentationEvent<Page>
    {
        public Page AddedPage { get; set; }

        public PageAddedEvent()
        {

        }

        public PageAddedEvent(Page page)
        {
            this.AddedPage = page;
        }
    }
}
