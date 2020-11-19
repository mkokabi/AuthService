using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        private static IServiceProvider _serviceProvider;

        static async Task Main(string[] args)
        {
            RegisterServices(args);
            var scope = _serviceProvider.CreateScope();
            await scope.ServiceProvider.GetRequiredService<ConsoleApp>().Run();
        }

        private static void RegisterServices(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddCommandLine(args)
                .Build();

            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddOptions<ConsoleAppConfig>()
                .Bind(configuration.GetSection("ConsoleAppConfig"));
            services.AddHttpClient();
            services.AddSingleton<ILoginService, LoginService>();
            services.AddSingleton<ConsoleApp>();
            _serviceProvider = services.BuildServiceProvider(true);
        }

        public class ConsoleApp
        {
            private readonly ILoginService loginService;
            private readonly IConfiguration configuration;

            public ConsoleApp(ILoginService loginService, IConfiguration configuration)
            {
                this.loginService = loginService;
                this.configuration = configuration;
            }

            public async Task Run()
            {
                if (configuration.GetValue<string>("action") == null)
                {
                    Console.WriteLine("No action provided.");
                    ShowHelp();
                    return;
                }
                if (configuration.GetValue<string>("action").Equals("login", StringComparison.InvariantCultureIgnoreCase))
                {
                    Console.WriteLine(await Login());
                }
            }

            private void ShowHelp()
            {
                Console.WriteLine("Usage: ");
                Console.WriteLine(@"--Action=Login --Username={string} --Password={string");
            }

            private async Task<string> Login()
            {
                var username = configuration.GetValue<string>("username");
                var password = configuration.GetValue<string>("password");

                return await loginService.Login(username, password);
            }
        }
    }
}
