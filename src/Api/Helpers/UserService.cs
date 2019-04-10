using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Helpers
{
    public class UserService : IUserService
    {
        // users injected via config for simplicity, store in a db with hashed passwords in production applications
        private readonly IList<User> _users;

        public UserService(IList<User> users)
        {
            _users = users;
        }
    
        public async Task<User> Authenticate(string username, string password)
        {
            var user = await Task.Run(() => _users.SingleOrDefault(x => x.Username == username && x.Password == password));

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so return user details without password
            user.Password = null;
            return user;
        }
    }
}