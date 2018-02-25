using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Events;

namespace Ertis.Infrastructure.Events
{
    public class AllModulesInitializedEvent : CompositePresentationEvent<AllModulesInitializedEvent>
    {
        public AllModulesInitializedEvent()
        {

        }
    }
}
