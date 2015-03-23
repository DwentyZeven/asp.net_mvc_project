using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Project.Interfaces;
using Project.Models;
using Project.Web.Extensions.Authentication;

namespace Project.Web.Controllers
{
    public class VkontakteController : AuthController
    {
        private const string ACCESS_TOKEN_SESSION = "AccessToken";

        private const string AUTHORIZE = "http://api.vk.com/oauth/authorize?client_id={0}&redirect_uri={1}";

        private const string GET_ACCESS_TOKEN = "https://api.vk.com/oauth/access_token?client_id={0}&client_secret={1}&code={2}";

        private static string VkontakteApplicationId
        {
            get { return ConfigurationManager.AppSettings["vkontakteApplicationId"]; }
        }

        private static string VkontakteApplicationSecret
        {
            get { return ConfigurationManager.AppSettings["vkontakteApplicationSecret"]; }
        }

        public bool IsVkontakteConsumerConfigured
        {
            get
            {
                return !string.IsNullOrEmpty(VkontakteApplicationId)
                       && !string.IsNullOrEmpty(VkontakteApplicationSecret);
            }
        }

        private readonly IVkontakteServices _vkontakteServices;

        [InjectionConstructor]
        public VkontakteController(IFormsAuthentication formsAuthentication, IUserRepository userRepository, IVkontakteServices vkontakteServices)
            : base(formsAuthentication, userRepository)
        {
            _vkontakteServices = vkontakteServices;
        }

        public ActionResult Index()
        {
            if (Request.Url != null)
            {
                var redirectTo = Request.Url.Scheme + "://" + Request.Url.Authority + Request.Url.AbsolutePath + "/auth";
                var request = string.Format(AUTHORIZE, VkontakteApplicationId, redirectTo);

                return Redirect(request);
            }
            return RedirectToRoute("SongIndex");
        }

        public ActionResult Auth()
        {
            if (Request.Params.AllKeys.Contains("code"))
            {
                User user = null;
                var request = GetRequestForAuthToken();

                if (request != null)
                    Session[ACCESS_TOKEN_SESSION] = _vkontakteServices.GetAccessToken(request);
                if (Session[ACCESS_TOKEN_SESSION] != null)
                    user = _vkontakteServices.GetUserInfo((string) Session[ACCESS_TOKEN_SESSION]);
                if (user != null)
                    user = userRepository.GetOrCreateUser(user);
                if (user != null && !user.IsBanned)
                {
                    userRepository.SetUserOnline(user.UserId);
                    formsAuthentication.SetAuthCookie(HttpContext, UserAuthenticationTicketBuilder.CreateAuthenticationTicket(user));
                    return RedirectToRoute("SongAdd");
                }
                if (user != null && user.IsBanned)
                    TempData["message"] = "Sorry, your account was banned";
            }

            if (Request.Params.AllKeys.Contains("error"))
            {
                TempData["error"] = Request.Params["error_description"];
            }
            return RedirectToRoute("SongIndex");
        }

        private string GetRequestForAuthToken()
        {
            if (Request.Url != null)
            {
                var code = Request.Params["code"];
                return string.Format(GET_ACCESS_TOKEN, VkontakteApplicationId, VkontakteApplicationSecret, code);
            }
            return null;
        }
    }
}
