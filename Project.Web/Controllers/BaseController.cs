using System.Web;
using System.Web.Mvc;
using Project.Web.Extensions.Attributes;

namespace Project.Web.Controllers
{
    [SetCulture]
    public class BaseController : Controller
    {
        public BaseController()
        {
        }

        public void SetTimeZone(short? offset)
        {
            if (!Request.IsAjaxRequest() || offset == null)
                throw new HttpException(404, "Not Found");

            Session["TimeZone"] = offset;
        }
    }
}