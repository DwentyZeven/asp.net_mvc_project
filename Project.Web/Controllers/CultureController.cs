using System.Web.Mvc;

namespace Project.Web.Controllers
{
    public class CultureController : BaseController
    {
        public ActionResult SetRussian()
        {
            Session["Culture"] = 1;

            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.ToString());

            return RedirectToRoute("SongIndex");
        }

        public ActionResult SetEnglish()
        {
            Session["Culture"] = 2;

            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.ToString());

            return RedirectToRoute("SongIndex");
        }
    }
}
