using System.Web;
using System.Web.Mvc;
using Project.Web.Extensions.Attributes;

namespace Project.Web.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class ElmahController : Controller
    {
        public ActionResult Index(string type)
        {
            return new ElmahResult(type);
        }

        public ActionResult Detail(string type)
        {
            return new ElmahResult(type);
        }
    }

    internal class ElmahResult : ActionResult
    {
        private readonly string _resourceType;

        public ElmahResult(string resourceType)
        {
            _resourceType = resourceType;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var factory = new Elmah.ErrorLogPageFactory();
            if (!string.IsNullOrEmpty(_resourceType))
            {
                var pathInfo = "." + _resourceType;
                HttpContext.Current.RewritePath(HttpContext.Current.Request.Path, pathInfo, HttpContext.Current.Request.QueryString.ToString());
            }

            var handler = factory.GetHandler(HttpContext.Current, null, null, null);
            handler.ProcessRequest(HttpContext.Current);
        }
    }
}
