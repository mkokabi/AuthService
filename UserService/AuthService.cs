using System.Threading.Tasks;
using UserServiceBase;

namespace UserService
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository userRepository;

        public AuthService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<int> CreateUser(string username, string password)
        {
            var result = await userRepository.UpsertUser(new User
            {
                Username = username,
                Password = password
            });
            return result;
        }

        public async Task<AuthResult> Login(string username, string password)
        {
            var result = await userRepository.GetUser(username);
            if (result.Password == password)
            {
                return new AuthResult
                {
                    Success = true,
                    Message = "Login was sucessful"
                };
            }
            return new AuthResult
            {
                Success = false,
                Message = "Login failed"
            };
        }
    }
}
