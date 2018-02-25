using Ertis.Infrastructure.Events;
using Ertis.Shared.Models;
using Ertis.Shared.Services.Contracts;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ertis.Shared.Commands
{
    public class DockViewCommand : ICommand
    {
        public DockableViewMenuItem vmi { get; set; }

        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="viewMenuItem"></param>
        public DockViewCommand(DockableViewMenuItem viewMenuItem)
        {
            vmi = viewMenuItem;
        }

        public void Execute(object parameter)
        {
            var eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<LeftSidePanelCollapseRequestEvent>().Publish(new LeftSidePanelCollapseRequestEvent());

            var windowNavigationService = ServiceLocator.Current.GetInstance<IWindowNavigationService>();
            windowNavigationService.NavigateView(vmi);
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
    }
}
