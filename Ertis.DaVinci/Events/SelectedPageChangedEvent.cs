using Ertis.DaVinci.HtmlModels;
using Microsoft.Practices.Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.Events
{
    public class SelectedPageChangedEvent : CompositePresentationEvent<Page>
    {
        public Page SelectedPage { get; set; }

        public SelectedPageChangedEvent()
        {

        }

        public SelectedPageChangedEvent(Page page)
        {
            this.SelectedPage = page;
        }
    }
}
