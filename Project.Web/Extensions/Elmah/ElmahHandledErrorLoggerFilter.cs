using System.Web.Mvc;
using Elmah;

namespace Project.Web.Extensions.Elmah
{
    public class ElmahHandledErrorLoggerFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            // Log only unhandled exceptions, because all other will be caught by ELMAH anyway
            if (filterContext.ExceptionHandled)
                ErrorSignal.FromCurrentContext().Raise(filterContext.Exception);
        }
    }
}