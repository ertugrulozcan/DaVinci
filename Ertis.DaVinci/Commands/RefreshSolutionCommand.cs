using Ertis.DaVinci.Services.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.Commands
{
    public class RefreshSolutionCommand : DelegateCommand
    {
        private static readonly Action ExecuteAction = delegate
        {
            var solutionService = ServiceLocator.Current.GetInstance<ISolutionService>();
            solutionService.RefreshWebSite();
        };

        public RefreshSolutionCommand() : base(ExecuteAction)
        {

        }
    }
}
