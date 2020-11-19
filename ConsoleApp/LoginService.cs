using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;

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

        public async Task<string> Login(string username, string pasword)
        {
            var httpClient = this.clientFactory.CreateClient();
            var httpResponse = await httpClient.GetAsync($"{options.Value.AuthApiURL}/Login?username={username}&password={pasword}");
            var response = await httpResponse.Content.ReadAsStringAsync();
            return await Task.FromResult($"API response: {response}");
        }
    }
}
