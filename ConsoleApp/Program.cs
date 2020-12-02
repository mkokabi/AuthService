using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SqlClient;
using UserServiceBase;
using UserServiceDynamoRepository;

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
            services.AddSingleton<IUserRepository, DynamoUserRepository>();
            services.AddSingleton<ConsoleApp>();
            _serviceProvider = services.BuildServiceProvider(true);
        }

        public class ConsoleApp
        {
            private readonly ILoginService loginService;
            private readonly IConfiguration configuration;
            private readonly IUserRepository userRepository;
            private readonly Dictionary<string, Func<Task<string>>> RunActions = new Dictionary<string, Func<Task<string>>>(StringComparer.InvariantCultureIgnoreCase);

            public ConsoleApp(ILoginService loginService, IConfiguration configuration, IUserRepository userRepository)
            {
                this.loginService = loginService;
                this.configuration = configuration;
                this.userRepository = userRepository;
                RunActions.Add("login", Login);
                RunActions.Add("migrate", Migrate);
                RunActions.Add("GetUsernames", GetUserNames);
            }

            public async Task Run()
            {
                var actionArg = configuration.GetValue<string>("action");
                if (actionArg == null)
                {
                    Console.WriteLine("No action provided.");
                    ShowHelp();
                    return;
                }

                if (!RunActions.TryGetValue(actionArg, out var action))
                {
                    Console.WriteLine("Invalid action.");
                    ShowHelp();
                    return;
                }

                Console.WriteLine(await action());
            }

            private void ShowHelp()
            {
                Console.WriteLine("Usage: ");
                Console.WriteLine(@"--Action=Login --Using={WebService|WebAPI} --Username={string} --Password={string");
                Console.WriteLine(@"--Action=GetUserNames");
                Console.WriteLine(@"--Action=Migrate --FromID={number}");
            }

            private async Task<string> Login()
            {
                var username = configuration.GetValue<string>("username");
                var password = configuration.GetValue<string>("password");

                if (configuration.GetValue<string>("using").Equals("WebService", StringComparison.InvariantCultureIgnoreCase))
                {
                    return await loginService.LoginUsingWebService(username, password);
                }
                else if (configuration.GetValue<string>("using").Equals("WebAPI", StringComparison.InvariantCultureIgnoreCase))
                {
                    return await loginService.LoginUsingWebApi(username, password);
                }
                return @"Invalid ""using"" arg";
            }

            private async Task<string> Migrate()
            {
                var fromId = configuration.GetValue<string>("fromid");
                string connStr = configuration.GetConnectionString("usersdbConnectionString");
                using (var conn = new SqlConnection(connStr))
                {
                    var commandText = "Select UserID, UserName, Password from Users where userId > @fromUserId";
                    conn.Open();
                    using (var cmd = new SqlCommand(commandText, conn))
                    {
                        cmd.Parameters.AddWithValue("fromUserId", fromId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var user = new User
                                {
                                    UserId = reader.GetInt32(0),
                                    Username = reader.GetString(1),
                                    Password = reader.GetString(2)
                                };

                                await userRepository.UpsertUser(user);
                            }
                        }
                    }
                }

                return await Task.FromResult<string>("1");
            }

            private async Task<string> GetUserNames()
            {
                var result = await loginService.GetUserNames();
                return await Task.FromResult("Usernames");
            }
        }
    }
}
