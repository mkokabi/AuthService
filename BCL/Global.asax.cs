using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using System;
using static BCL.AuthWebServiceBCL;
using ServiceCollection = Microsoft.Extensions.DependencyInjection.ServiceCollection;

namespace BCL
{
    public class Global : System.Web.HttpApplication
    {
        internal static IServiceProvider _serviceProvider;

        protected void Application_Start(object sender, EventArgs e)
        {
            //
            // Setup configuration
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            //
            // Setup application services + feature management
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton(configuration)
                    .AddFeatureManagement();
            services.AddOptions<BCLAppConfig>()
                .Bind(configuration.GetSection("BCLAppConfig"));
            services.AddSingleton<BCLApp>();

            _serviceProvider = services.BuildServiceProvider();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}