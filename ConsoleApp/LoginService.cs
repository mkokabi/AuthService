using AutWebServiceReference;
using IdentityModel.Client;
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

        public async Task<string> LoginUsingWebApi(string username, string password)
        {
            var wapiUrl = options.Value.AuthApiURL;
            var httpClient = this.clientFactory.CreateClient();
            var requestBody = System.Text.Json.JsonSerializer.Serialize(new
            {
                username = username,
                password = password
            });
            var stringContent = new StringContent(requestBody, encoding: System.Text.Encoding.UTF8, mediaType: "application/json");
            var httpResponse = await httpClient.PostAsync($"{wapiUrl}/Login", stringContent);

            var response = await httpResponse.Content.ReadAsStringAsync();
            return $"API response: {response}";
        }

        public async Task<string[]> GetUserNames()
        {
            var wapiUrl = options.Value.AuthApiURL;

            var httpClient = this.clientFactory.CreateClient();
            var disco = await httpClient.GetDiscoveryDocumentAsync("https://localhost:5101");
            if (disco.IsError)
            {
                throw new System.Exception("Can not get the disco");
            }

            var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "client",
                ClientSecret = "secret",
                Scope = "api1"
            });

            if (tokenResponse.IsError)
            {
                throw new System.Exception("Can not get token");
            }

            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var httpResponse = await httpClient.GetAsync($"{wapiUrl}/GetUsers");
            return null;
        }

        public async Task<string> LoginUsingWebService(string username, string pasword)
        {
            var webServiceClient = new AuthWebServiceSoapClient(EndpointConfiguration.AuthWebServiceSoap12, options.Value.AuthWebServiceURL);
            var response = await webServiceClient.LoginAsync(username, pasword);
            return $"WebService response: {response}";
        }
    }
}
