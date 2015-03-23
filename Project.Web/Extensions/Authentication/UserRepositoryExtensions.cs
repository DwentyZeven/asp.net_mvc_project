using Project.Interfaces;
using Project.Models;
using Project.Web.Models;

namespace Project.Web.Extensions.Authentication
{
    public static class UserRepositoryExtensions
    {
        public static User GetUserFromIdentity(this IUserRepository services, ProjectIdentity identity)
        {
            var user = services.GetUser(identity.UserId);
            return user;
        }
    }
}