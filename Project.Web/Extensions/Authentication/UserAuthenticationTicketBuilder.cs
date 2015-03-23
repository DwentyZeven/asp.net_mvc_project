using System;
using System.Web.Security;
using Project.Models;
using Project.Web.Models;

namespace Project.Web.Extensions.Authentication
{
    public class UserAuthenticationTicketBuilder
    {
        public static FormsAuthenticationTicket CreateAuthenticationTicket(User user)
        {
            UserInfo userInfo = CreateUserContextFromUser(user);

            var ticket = new FormsAuthenticationTicket(
                1,
                user.Link,
                DateTime.Now,
                DateTime.Now.Add(FormsAuthentication.Timeout),
                false,
                userInfo.ToString()
            );

            return ticket;
        }

        private static UserInfo CreateUserContextFromUser(User user)
        {
            var userContext = new UserInfo
            {
                UserId = user.UserId,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Username = user.Username,
                Link = user.Link,
                Role = user.Role,
                IsWarned = user.IsWarned,
                IsBanned = user.IsBanned
            };

            return userContext;
        }
    }
}