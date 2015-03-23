using Microsoft.Practices.Unity;
using Project.Interfaces;
using Project.Models;
using Project.Web.Extensions.Authentication;

namespace Project.Web.Controllers
{
    public class AuthorizedController : BaseController
    {
        protected readonly IUserRepository userRepository;

        [InjectionConstructor]
        public AuthorizedController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        
        private User _currentUser;

        protected long CurrentUserId
        {
            get { return User.ProjectIdentity().UserId; }
        }

        public User CurrentUser
        {
            get { return _currentUser ?? (_currentUser = userRepository.GetUserFromIdentity(User.ProjectIdentity())); }
        }
    }
}
