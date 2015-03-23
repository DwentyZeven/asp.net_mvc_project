using System.Collections.ObjectModel;
using Project.Models;

namespace Project.Interfaces
{
    public interface IUserRepository
    {
        User GetUser(long? userId);

        User GetFbUser(long facebookId);

        User GetVkUser(long vkontakteId);

        User GetOrCreateUser(User newUser);

        ReadOnlyCollection<User> GetAllUsers(bool paged);

        long CountAllUsers();

        User CreateUser(User newUser);

        void UpdateUser(User updatedUser);

        void DeleteUser(long userId);

        void SetUserOnline(long userId);

        void SetUserOffline(long userId);

        void SetPaginationParams(int pageIndex, int pageSize);
    }
}
