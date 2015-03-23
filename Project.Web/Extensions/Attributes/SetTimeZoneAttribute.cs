using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Project.Web.Extensions.Attributes
{
    //public class SetTimeZoneAttribute : FilterAttribute, IActionFilter
    //{
    //    public void OnActionExecuting(ActionExecutingContext filterContext)
    //    {
    //        var session = filterContext.HttpContext.Session;
    //        if (session != null && session["TimeZone"] == null)
    //        {
    //            var ip = GetIpAddress();
    //            var request = string.Format(GET_USER_INFO, ip);
    //            var response = GetHttpResponse(request);
    //            session["TimeZone"] = response;
    //        }
    //    }

    //    public void OnActionExecuted(ActionExecutedContext filterContext)
    //    {
    //    }

    //    private static string GetIpAddress()
    //    {
    //        var ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

    //        if (string.IsNullOrEmpty(ip))
    //        {
    //            ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
    //        }

    //        return ip;
    //    }

    //    private static string GetHttpResponse(string request)
    //    {
    //        try
    //        {
    //            var httpRequest = (HttpWebRequest) WebRequest.Create(request);
    //            var httpResponse = (HttpWebResponse) httpRequest.GetResponse();
    //            var httpResponseStream = httpResponse.GetResponseStream();

    //            return httpResponseStream != null ? new StreamReader(httpResponseStream, Encoding.UTF8).ReadToEnd() : null;
    //        }
    //        catch (InvalidOperationException e)
    //        {
    //        }
    //    }
    //}
}