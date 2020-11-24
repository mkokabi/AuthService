using System.Threading.Tasks;

namespace UserServiceBase
{
    public interface IUserRepository
    {
        Task<int> UpsertUser(User user);
        Task<User> GetUser(string username);
    }

    public class User
    {
        public int? UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
