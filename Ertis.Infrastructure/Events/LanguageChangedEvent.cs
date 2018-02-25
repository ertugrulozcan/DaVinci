using Microsoft.Practices.Prism.Events;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Infrastructure.Events
{
    public class LanguageChangedEvent : CompositePresentationEvent<CultureInfo>
    {
        public CultureInfo Culture { get; private set; }

        public LanguageChangedEvent()
        {

        }

        public LanguageChangedEvent(CultureInfo cultureInfo)
        {
            this.Culture = cultureInfo;
        }
    }
}
