using Ertis.Shared.ScreenManagement;
using Microsoft.Practices.Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Shared.Events
{
    public class CurrentScreenChangedEvent : CompositePresentationEvent<CurrentScreenChangeEventArgs>
    {
        public WpfScreen NewScreen { get; private set; }
        public WpfScreen OldScreen { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public CurrentScreenChangedEvent()
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="args"></param>
        public CurrentScreenChangedEvent(CurrentScreenChangeEventArgs args)
        {
            this.NewScreen = args.NewScreen;
            this.OldScreen = args.OldScreen;
        }
    }
}
