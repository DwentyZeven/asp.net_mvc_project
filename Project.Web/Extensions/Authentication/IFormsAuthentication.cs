using System.Web;
using System.Web.Security;

namespace Project.Web.Extensions.Authentication
{
    public interface IFormsAuthentication
    {
        void SignOut();

        void SetAuthCookie(string userName, bool persistent);

        void SetAuthCookie(HttpContextBase httpContext, FormsAuthenticationTicket authenticationTicket);

        void SetAuthCookie(HttpContext httpContext, FormsAuthenticationTicket authenticationTicket);

        FormsAuthenticationTicket Decrypt(string encryptedTicket);
    }
}
