using AutWebServiceReference;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;
using static AutWebServiceReference.AuthWebServiceSoapClient;

namespace ConsoleApp
{
    class LoginService : ILoginService
    {
        private readonly IHttpClientFactory clientFactory;
        private readonly IOptions<ConsoleAppConfig> options;

        public LoginService(IHttpClientFactory clientFactory, IOptions<ConsoleAppConfig> options)
        {
            this.clientFactory = clientFactory;
            this.options = options;
        }

        public async Task<string> LoginUsingWebApi(string username, string pasword)
        {
            var httpClient = this.clientFactory.CreateClient();
            var httpResponse = await httpClient.GetAsync($"{options.Value.AuthApiURL}/Login?username={username}&password={pasword}");
            var response = await httpResponse.Content.ReadAsStringAsync();
            return $"API response: {response}";
        }

        public async Task<string> LoginUsingWebService(string username, string pasword)
        {
            var webServiceClient = new AuthWebServiceSoapClient(EndpointConfiguration.AuthWebServiceSoap12, options.Value.AuthWebServiceURL);
            var response = await webServiceClient.LoginAsync(username, pasword);
            return $"WebService response: {response}";
        }
    }
}
