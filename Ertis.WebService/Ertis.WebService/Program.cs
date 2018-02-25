using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Ertis.WebService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            try
            {
                return WebHost.CreateDefaultBuilder(args)
                              .UseKestrel()
                              .UseContentRoot(Directory.GetCurrentDirectory())
                              .UseIISIntegration()
                              .UseStartup<Startup>()
                              .UseApplicationInsights()
                              .Build();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
