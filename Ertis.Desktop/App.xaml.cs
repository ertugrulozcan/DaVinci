using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Ertis.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Bootstrapper ApplicationBootstrapper;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ApplicationBootstrapper = new Bootstrapper();
            ApplicationBootstrapper.Run();
        }


        protected override void OnExit(ExitEventArgs e)
        {
            //bootstrapper.shell.eventAggregator.GetEvent<ApplicationCloseEvent>().Publish(true);

            base.OnExit(e);
        }
    }
}
