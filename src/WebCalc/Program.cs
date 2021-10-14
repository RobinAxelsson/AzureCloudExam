using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebCalc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IServiceCollection services = null;

            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
                .ConfigureServices(s => services = s);
            var host = builder.Build();

            host.Run();
        }
    }
}
