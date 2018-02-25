using Microsoft.Practices.Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ertis.Localization.Events
{
    public class FlowDirectionChangeRequestEvent : CompositePresentationEvent<FlowDirection>
    {
        public FlowDirection Direction { get; set; }

        public FlowDirectionChangeRequestEvent()
        {

        }

        public FlowDirectionChangeRequestEvent(FlowDirection direction)
        {
            this.Direction = direction;
        }
    }
}
