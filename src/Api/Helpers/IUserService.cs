using System.Threading.Tasks;

namespace Api.Helpers
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
    }
}