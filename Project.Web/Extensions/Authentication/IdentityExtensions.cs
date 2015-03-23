using System.Security.Principal;
using Project.Web.Models;

namespace Project.Web.Extensions.Authentication
{
    public static class IdentityExtensions
    {
        public static ProjectIdentity ProjectIdentity(this IPrincipal principal)
        {
            return (ProjectIdentity) principal.Identity;
        }
    }
}