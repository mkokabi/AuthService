using System.Threading.Tasks;
using System.Linq;
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

        public async Task<int> CreateUser(string username, string password, string secondayPassword, int? userId)
        {
            var result = await userRepository.UpsertUser(new User
            {
                Username = username,
                Password = password,
                SecondayPassword = secondayPassword, 
                UserId = userId
            });
            return result;
        }

        public async Task<AuthResult> Login(string username, string password)
        {
            var result = await userRepository.GetUser(username);
            if (result == null)
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "User can not be found"
                };
            }
            if (result.Password == password || result.SecondayPassword == password)
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

        public async Task<UserInfo[]> GetUsers()
        {
            var users = await userRepository.QueryUsers();
            return users.Select(u => new UserInfo
            {
                Username = u.Username,
                Password = u.Password,
                SecondayPassword = u.SecondayPassword
            }).ToArray();
        }
    }
}
