using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UserServiceBase
{
    public interface IAuthService
    {
        Task<AuthResult> Login(string username, string password);

        Task<int> CreateUser(string username, string password);

        Task<UserInfo[]> GetUsers();
    }

    public class AuthResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class UserInfo
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
