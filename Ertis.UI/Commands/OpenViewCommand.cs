using Ertis.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.ServiceLocation;
using Ertis.Shared.Services.Contracts;
using Microsoft.Practices.Prism.Events;
using Ertis.Infrastructure.Events;

namespace Ertis.Shared.Commands
{
    public class OpenViewCommand : ICommand
    {
        public ViewMenuItem vmi { get; set; }

        public event EventHandler CanExecuteChanged;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="viewMenuItem"></param>
        public OpenViewCommand(ViewMenuItem viewMenuItem)
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
