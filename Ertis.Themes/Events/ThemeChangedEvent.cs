using Microsoft.Practices.Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Themes.Events
{
    public class ThemeChangedEvent : CompositePresentationEvent<Theme>
    {
        public ThemeChangedEvent()
        {

        }

        public ThemeChangedEvent(Theme theme)
        {

        }
    }
}
