using BCL.AuthWebServiceReference;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using System.Net.Http;
using System.ServiceModel;
using System.Web.Services;

namespace BCL
{
    /// <summary>
    /// Summary description for AuthWebServiceBCL
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class AuthWebServiceBCL : System.Web.Services.WebService
    {
        [WebMethod]
        public string Login(string username, string password)
        {
            var scope = Global._serviceProvider.CreateScope();
            return scope.ServiceProvider.GetRequiredService<BCLApp>().Login(username, password);
        }
    }

    public class BCLApp
    {
        private readonly IOptions<BCLAppConfig> options;
        private readonly IFeatureManager featureManager;

        public BCLApp(IOptions<BCLAppConfig> options, IFeatureManager featureManager)
        {
            this.options = options;
            this.featureManager = featureManager;
        }

        public string Login(string username, string password)
        {
            var callWebService = featureManager.IsEnabledAsync("CallWebService").Result;
            if (callWebService)
            {
                var wsUrl = options.Value.AuthWebServiceURL;
                var webServiceClient = new AuthWebServiceSoapClient();
                webServiceClient.Endpoint.Address = new EndpointAddress(wsUrl);
                var response = webServiceClient.Login(username, password);
                return $"WebService response from BCL : {response}";
            }
            else
            {
                var wapiUrl = options.Value.AuthApiURL;
                var httpClient = new HttpClient();
                var httpResponse = httpClient.GetAsync($"{wapiUrl}/Login?username={username}&password={password}").Result;
                var response = httpResponse.Content.ReadAsStringAsync().Result;
                return $"WebAPI response from BCL : {response}";
            }
        }
    }
}
