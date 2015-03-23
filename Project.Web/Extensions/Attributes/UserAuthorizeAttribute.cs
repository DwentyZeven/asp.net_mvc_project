using System;
using System.Web;
using System.Web.Mvc;
using Project.Web.Models;

namespace Project.Web.Extensions.Attributes
{
    public class UserAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            var identity = httpContext.User.Identity as ProjectIdentity;

            if (identity == null || !identity.IsAuthenticated || identity.IsBanned)
                return false;
            
            return true;
        }
    }
}