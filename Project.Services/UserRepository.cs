using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using Project.Interfaces;
using Project.Models;
using Project.Services.Properties;

namespace Project.Services
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public User GetUser(long? userId)
        {
            if (userId == null) return null;

            try
            {
                return GetDbSet<User>().FirstOrDefault(u => u.UserId == userId);
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToGetUserExceptionMessage, e);
            }
        }

        public User GetFbUser(long facebookId)
        {
            try
            {
                return GetDbSet<User>().FirstOrDefault(u => u.FacebookId == facebookId);
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToGetUserExceptionMessage, e);
            }
        }

        public User GetVkUser(long vkontakteId)
        {
            try
            {
                return GetDbSet<User>().FirstOrDefault(u => u.VkontakteId == vkontakteId);
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToGetUserExceptionMessage, e);
            }
        }

        public User GetOrCreateUser(User newUser)
        {
            try
            {
                var user = (newUser.FacebookId != 0) ? GetFbUser(newUser.FacebookId) : GetVkUser(newUser.VkontakteId);
                return user ?? CreateUser(newUser);
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToGetUserExceptionMessage, e);
            }
        }

        public ReadOnlyCollection<User> GetAllUsers(bool paged)
        {
            try
            {
                var users = GetDbSet<User>().OrderByDescending(u => u.UserId);

                if (paged)
                    users = TakePageItems(users);

                return new ReadOnlyCollection<User>(users.ToList());
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToGetUsersExceptionMessage, e);
            }
        }

        public long CountAllUsers()
        {
            try
            {
                return GetDbSet<User>().LongCount();
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToCountUsersExceptionMessage, e);
            }
        }

        public User CreateUser(User newUser)
        {
            try
            {
                var user = GetDbSet<User>().Add(newUser);
                UnitOfWork.SaveChanges();
                return user;
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToCreateUserExceptionMessage, e);
            }
        }

        public void UpdateUser(User updatedUser)
        {
            try
            {
                var user = GetUser(updatedUser.UserId);
                if (user == null) return;

                user.FacebookId = updatedUser.FacebookId;
                user.VkontakteId = updatedUser.VkontakteId;
                user.Firstname = updatedUser.Firstname.Trim();
                user.Lastname = updatedUser.Lastname.Trim();
                user.Username = (updatedUser.Username != null) ? updatedUser.Username.Trim() : null;
                user.Gender = updatedUser.Gender;
                user.Email = updatedUser.Email;
                user.Link = updatedUser.Link;
                user.PhotoLink = updatedUser.PhotoLink;
                user.Role = updatedUser.Role;
                user.IsWarned = updatedUser.IsWarned;
                user.IsBanned = updatedUser.IsBanned;

                SetEntityState(user, EntityState.Modified);
                UnitOfWork.SaveChanges();
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToUpdateUserExceptionMessage, e);
            }
        }

        public void DeleteUser(long userId)
        {
            try
            {
                var user = GetUser(userId);
                if (user == null) return;

                GetDbSet<User>().Remove(user);
                UnitOfWork.SaveChanges();
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToDeleteUserExceptionMessage, e);
            }
        }

        public void SetUserOnline(long userId)
        {
            try
            {
                var user = GetUser(userId);
                if (user == null) return;

                user.IsOnline = true;

                SetEntityState(user, EntityState.Modified);
                UnitOfWork.SaveChanges();
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToSetUserOnlineExceptionMessage, e);
            }
        }

        public void SetUserOffline(long userId)
        {
            try
            {
                var user = GetUser(userId);
                if (user == null) return;

                user.IsOnline = false;

                SetEntityState(user, EntityState.Modified);
                UnitOfWork.SaveChanges();
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToSetUserOfflineExceptionMessage, e);
            }
        }
    }
}
