﻿using Microsoft.Practices.Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Infrastructure.Events
{
    public class LeftSidePanelLoadedEvent : CompositePresentationEvent<LeftSidePanelLoadedEvent>
    {
        public LeftSidePanelLoadedEvent()
        {

        }
    }
}