using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;

namespace Project.Web.Extensions.Attributes
{
    public class SetCultureAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var cultureNumber = GetCurrentLanguage(filterContext);

            if (filterContext.HttpContext.Session != null)
                filterContext.HttpContext.Session["Culture"] = cultureNumber;

            var culture = new CultureInfo(cultureNumber == 1 ? "ru" : "en");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        private static short GetCurrentLanguage(ActionExecutingContext filterContext)
        {
            var cultures = new List<short> { 1, 2 };
            var session = filterContext.RequestContext.HttpContext.Session;

            if (session != null && session["Culture"] != null)
            {
                var sessionCulture = Convert.ToInt16(session["Culture"]);
                if (cultures.Contains(sessionCulture))
                    return sessionCulture;
            }

            var cultureCode = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            if (cultureCode == "ru" || // Россия
                cultureCode == "be" || // Беларусь
                cultureCode == "uk" || // Украина
                cultureCode == "mo" || // Молдавия
                cultureCode == "et" || // Эстония
                cultureCode == "lv" || // Латвия
                cultureCode == "lt" || // Литва
                cultureCode == "ka" || // Грузия
                cultureCode == "hy" || // Армения
                cultureCode == "az" || // Азербайджан
                cultureCode == "kk" || // Казахстан
                cultureCode == "tk" || // Туркменистан
                cultureCode == "uz" || // Узбекистан
                cultureCode == "ky" || // Киргизия
                cultureCode == "tg")   // Таджикистан
                return 1;

            return 2;
        }

    }
}