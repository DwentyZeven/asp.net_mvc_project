using System;
using System.Web;
using System.Web.Mvc;

namespace Project.Web.Extensions.Attributes
{
    public class CacheFilterAttribute : ActionFilterAttribute
    {
        public int Duration { get; set; }

        public CacheFilterAttribute()
        {
            Duration = 10;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (Duration <= 0) return;

            HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;
            TimeSpan cacheDuration = TimeSpan.FromSeconds(Duration);

            cache.SetNoStore();
            cache.SetCacheability(HttpCacheability.NoCache);
            cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            cache.AppendCacheExtension("post-check=0, pre-check=0");
            //cache.SetVaryByCustom("lang");
            //cache.SetExpires(DateTime.Now.Add(cacheDuration));
            //cache.SetMaxAge(cacheDuration);
        }
    }
}