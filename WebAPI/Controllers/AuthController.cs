using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Linq;
using UserServiceBase;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        public class UserCredentials
        {
            [JsonPropertyName("username")]
            public string Username { get; set; }
            [JsonPropertyName("password")]
            public string Password { get; set; }
        }

        [Authorize]
        [HttpPost("Login")]
        public async Task<AuthResult> Login([FromBody]UserCredentials userCredentials)
        {
            return await authService.Login(userCredentials.Username, userCredentials.Password);
        }

        public class UserDetails
        {
            [JsonPropertyName("username")]
            public string Username { get; set; }
            [JsonPropertyName("password")]
            public string Password { get; set; }
            [JsonPropertyName("secondayPassword")]
            public string SecondayPassword { get; set; }
            [JsonPropertyName("userId")]
            public int? UserId { get; set; }
        }

        [HttpPost("CreateUser")]
        public async Task<int> CreateUser([FromBody]UserDetails userDetails)
        {
            return await authService.CreateUser(userDetails.Username, userDetails.Password, userDetails.SecondayPassword, userDetails.UserId);
        }

        [Authorize]
        [HttpGet("GetUsers")]
        public async Task<UserDetails[]> Get()
        {
            var userInfos = await authService.GetUsers();
            return userInfos.Select(u => new UserDetails { Username = u.Username, Password = u.Password }).ToArray();
        }
    }
}
