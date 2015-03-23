using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Project.Interfaces;
using Project.Models;
using Project.Web.Extensions.Authentication;

namespace Project.Web.Controllers
{
    public class FacebookController : AuthController
    {
        private const string ACCESS_TOKEN_SESSION = "AccessToken";

        private const string AUTHORIZE = "https://www.facebook.com/dialog/oauth?client_id={0}&redirect_uri={1}";

        private const string GET_ACCESS_TOKEN = "https://graph.facebook.com/oauth/access_token?client_id={0}&redirect_uri={1}&client_secret={2}&code={3}";

        private static string FacebookApplicationId
        {
            get { return ConfigurationManager.AppSettings["facebookApplicationId"]; }
        }

        private static string FacebookApplicationSecret
        {
            get { return ConfigurationManager.AppSettings["facebookApplicationSecret"]; }
        }

        private static string FacebookApplicationKey
        {
            get { return ConfigurationManager.AppSettings["facebookApplicationKey"]; }
        }

        public bool IsFacebookConsumerConfigured
        {
            get
            {
                return !string.IsNullOrEmpty(FacebookApplicationId)
                       && !string.IsNullOrEmpty(FacebookApplicationSecret)
                       && !string.IsNullOrEmpty(FacebookApplicationKey);
            }
        }

        private readonly IFacebookServices _facebookServices;

        [InjectionConstructor]
        public FacebookController(IFormsAuthentication formsAuthentication, IUserRepository userRepository, IFacebookServices facebookServices)
            : base(formsAuthentication, userRepository)
        {
            _facebookServices = facebookServices;
        }

        public ActionResult Index()
        {
            if (Request.Url != null)
            {
                var redirectTo = Request.Url.Scheme + "://" + Request.Url.Authority + Request.Url.AbsolutePath + "/auth";
                var request = string.Format(AUTHORIZE, FacebookApplicationId, redirectTo);

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
                    Session[ACCESS_TOKEN_SESSION] = _facebookServices.GetAccessToken(request);
                if (Session[ACCESS_TOKEN_SESSION] != null)
                    user = _facebookServices.GetUserInfo((string) Session[ACCESS_TOKEN_SESSION]);
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
                var redirectTo = Request.Url.Scheme + "://" + Request.Url.Authority + Request.Url.AbsolutePath;
                var code = Request.Params["code"];
                return string.Format(GET_ACCESS_TOKEN, FacebookApplicationId, redirectTo, FacebookApplicationSecret, code);
            }
            return null;
        }
    }
}
