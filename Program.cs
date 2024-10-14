using Microsoft.Extensions.Hosting;

namespace GtvApiHub
{
    internal class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var serviceProvider = host.Services;

            Console.WriteLine("Hello, World!");

            await host.RunAsync(); // Ensure the method is awaited

            return 0; // Ensure a value is returned
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
               .ConfigureServices((_, services) =>
               {
                   var startup = new Startup();
                   startup.ConfigureServices(services);
               });
    }
}
