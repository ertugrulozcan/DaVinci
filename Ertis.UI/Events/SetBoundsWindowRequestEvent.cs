using Ertis.Shared.ScreenManagement;
using Microsoft.Practices.Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Shared.Events
{
    public class SetBoundsWindowRequestEvent : CompositePresentationEvent<WpfScreen>
    {
        public WpfScreen Screen { get; private set; }

        public SetBoundsWindowRequestEvent()
        {

        }

        public SetBoundsWindowRequestEvent(WpfScreen screen)
        {
            this.Screen = screen;
        }
    }
}
