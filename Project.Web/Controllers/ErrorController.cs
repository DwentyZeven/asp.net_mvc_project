using System.Web;
using System.Web.Mvc;
using Project.Web.Extensions.Attributes;

namespace Project.Web.Controllers
{
    [HandleError]
    [CompressFilter(Order = 3)]
    [CacheFilter(Duration = 0, Order = 2)]
    [OutputCache(Duration = 0, VaryByParam = "none", Order = 1)]
    public class ErrorController : BaseController
    {
        public ActionResult Index()
        {
            ViewData["Description"] = Project.Web.Properties.Resources.ErrorIndex;
            return View();
        }

        public ActionResult Error404()
        {
            ViewData["Description"] = Project.Web.Properties.Resources.Error404;
            return View("Index");
        }

        public ActionResult Error500()
        {
            ViewData["Description"] = Project.Web.Properties.Resources.Error500;
            return View("Index");
        }

        public ActionResult Test(int number)
        {
            throw new HttpException(number, "Error " + number);
        }
    }
}
