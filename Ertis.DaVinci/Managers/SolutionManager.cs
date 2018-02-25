using Ertis.DaVinci.Models;
using Ertis.DaVinci.Services.Interfaces;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.Managers
{
    public class SolutionManager
    {
        private static SolutionManager self;

        public static SolutionManager Current
        {
            get
            {
                if (self == null)
                    self = new SolutionManager();

                return self;
            }
        }

        private ISolutionService SolutionService { get; set; }

        public Solution CurrentSolution
        {
            get
            {
                if (this.SolutionService == null)
                    return null;

                return this.SolutionService.CurrentSolution;
            }
        }

        public SolutionManager()
        {
            this.SolutionService = ServiceLocator.Current.GetInstance<ISolutionService>();
        }

        public SiteSettings GetCurrentSiteSettings()
        {
            if (this.CurrentSolution == null)
                return new SiteSettings();

            if (this.CurrentSolution.ProjectList == null || this.CurrentSolution.ProjectList.Count == 0)
                return new SiteSettings();

            if (this.CurrentSolution.ProjectList.First().Site == null)
                return new SiteSettings();

            return this.CurrentSolution.ProjectList.First().Site.SiteSettings;
        }
    }
}
