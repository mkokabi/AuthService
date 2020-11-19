using System.Threading.Tasks;
using UserServiceBase;

namespace UserService
{
    public class AuthService : IAuthService
    {
        public Task<AuthResult> Login(string username, string password)
        {
            return Task.FromResult(new AuthResult
            {
                Success = true,
                Message = "Login was sucessful"
            });
        }
    }
}
