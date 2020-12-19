using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

using DreamRecorder.ToolBox.General;

using Microsoft.AspNetCore.Hosting ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting ;

namespace DreamRecorder . Directory . Services . ApiService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).UseServiceProviderFactory(StaticServiceFac)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }


}
