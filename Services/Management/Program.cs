using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;

using Microsoft.AspNetCore.Hosting ;
using Microsoft.Extensions.Hosting ;

namespace DreamRecorder . Directory . Services . Management
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
