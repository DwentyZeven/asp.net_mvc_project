using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Project.Interfaces;
using Project.Web.Extensions.Attributes;
using Project.Web.Extensions.Authentication;
using Project.Web.Models;

namespace Project.Web.Controllers
{
    [CompressFilter(Order = 3)]
    [CacheFilter(Duration = 0, Order = 2)]
    [OutputCache(Duration = 0, VaryByParam = "none", Order = 1)]
    public class AuthController : BaseController
    {
        protected readonly IFormsAuthentication formsAuthentication;

        protected readonly IUserRepository userRepository;

        [InjectionConstructor]
        public AuthController(IFormsAuthentication formsAuthentication, IUserRepository userRepository)
        {
            this.formsAuthentication = formsAuthentication;
            this.userRepository = userRepository;
        }

        public ActionResult SignIn()
        {
            var identity = User.Identity as ProjectIdentity;
            if (identity != null && identity.IsAuthenticated)
            {
                return identity.IsBanned ? RedirectToRoute("AuthSignOut") : RedirectToRoute("SongAdd");
            }

            return View();
        }

        public ActionResult SignOut()
        {
            userRepository.SetUserOffline(((ProjectIdentity) User.Identity).UserId);
            formsAuthentication.SignOut();
            return RedirectToRoute("SongIndex");
        }
    }
}
