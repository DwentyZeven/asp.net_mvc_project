using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Elmah;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Project.Db;
using Project.Db.Initializers;
using Project.Interfaces;
using Project.Services;
using Project.Web.Controllers;
using Project.Web.Extensions.Authentication;
using Project.Web.Extensions.Elmah;
using Project.Web.Models;

namespace Project.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static IUnityContainer Container
        {
            get
            {
                var container = HttpContext.Current.Items["Container"] as IUnityContainer;
                if (container == null)
                {
                    container = new UnityContainer()
                        .RegisterType<IControllerActivator, UnityControllerActivator>(new ContainerControlledLifetimeManager())
                        .RegisterType<ITicketRepository, TicketRepository>(new ContainerControlledLifetimeManager())
                        .RegisterType<IUserRepository, UserRepository>(new ContainerControlledLifetimeManager())
                        .RegisterType<IFacebookServices, FacebookServices>(new ContainerControlledLifetimeManager())
                        .RegisterType<IVkontakteServices, VkontakteServices>(new ContainerControlledLifetimeManager())
                        .RegisterType<IUnitOfWork, ProjectDbContext>(new ContainerControlledLifetimeManager())
                        .RegisterType<IDatabaseInitializer, DatabaseInitializer>(new ContainerControlledLifetimeManager())
                        .RegisterType<IFormsAuthentication, DefaultFormsAuthentication>(new ContainerControlledLifetimeManager());
                    HttpContext.Current.Items["Container"] = container;
                }
                return container;
            }
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ElmahHandledErrorLoggerFilter());
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.([iI][cC][oO]|[gG][iI][fF])(/.*)?" });

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "TicketList",
                "",
                new { controller = "Ticket", action = "List" },
                new[] { "Project.Web.Controllers" }
            );

            routes.MapRoute(
                "TicketDetails",
                "ticket/{ticketId}",
                new { controller = "Ticket", action = "Details", tikcetId = UrlParameter.Optional },
                new[] { "Project.Web.Controllers" }
            );

            routes.MapRoute(
                "TicketByUser",
                "user/{userId}",
                new { controller = "Ticket", action = "ByUser", userId = UrlParameter.Optional },
                new[] { "Project.Web.Controllers" }
            );

            routes.MapRoute(
                "TicketAdd",
                "add_ticket",
                new { controller = "Ticket", action = "Add" },
                new[] { "Project.Web.Controllers" }
            );

            routes.MapRoute(
                "AuthSignIn",
                "login",
                new { controller = "Auth", action = "SignIn" },
                new[] { "Project.Web.Controllers" }
            );

            routes.MapRoute(
                "AuthSignOut",
                "logout",
                new { controller = "Auth", action = "SignOut" },
                new[] { "Project.Web.Controllers" }
            );

            routes.MapRoute(
                "FacebookIndex",
                "facebook",
                new { controller = "Facebook", action = "Index" },
                new[] { "Project.Web.Controllers" }
            );

            routes.MapRoute(
                "FacebookAuth",
                "facebook/auth",
                new { controller = "Facebook", action = "Auth" },
                new[] { "Project.Web.Controllers" }
            );

            routes.MapRoute(
                "VkontakteIndex",
                "vkontakte",
                new { controller = "Vkontakte", action = "Index" },
                new[] { "Project.Web.Controllers" }
            );

            routes.MapRoute(
                "VkontakteAuth",
                "vkontakte/auth",
                new { controller = "Vkontakte", action = "Auth" },
                new[] { "Project.Web.Controllers" }
            );

            routes.MapRoute(
                "CultureSetRussian",
                "ru",
                new { controller = "Culture", action = "SetRussian" },
                new[] { "Project.Web.Controllers" }
            );

            routes.MapRoute(
                "CultureSetEnglish",
                "en",
                new { controller = "Culture", action = "SetEnglish" },
                new[] { "Project.Web.Controllers" }
            );

            routes.MapRoute(
                "BaseSetTimeZone",
                "timezone",
                new { controller = "Base", action = "SetTimeZone", offset = UrlParameter.Optional },
                new[] { "Project.Web.Controllers" }
            );

            //routes.MapRoute(
            //    "ErrorTest",
            //    "fromsongtesterrorpage/{number}",
            //    new { controller = "Error", action = "Test", number = UrlParameter.Optional },
            //    new[] { "Project.Web.Controllers" }
            //);

            routes.MapRoute(
                "Error404",
                "{*url}",
                new { controller = "Error", action = "Error404", id = UrlParameter.Optional },
                new[] { "Project.Web.Controllers" }
            );
        }

        public override void Init()
        {
            PostAuthenticateRequest += PostAuthenticateRequestHandler;
            base.Init();
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            InitializeDependencyInjectionContainer();
            InitializeDatabase();
        }

        private void PostAuthenticateRequestHandler(object sender, EventArgs e)
        {
            HttpCookie authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
            {
                var formsAuthentication = ServiceLocator.Current.GetInstance<IFormsAuthentication>();

                var ticket = formsAuthentication.Decrypt(authCookie.Value);
                var projectIdentity = new ProjectIdentity(ticket);
                Context.User = new GenericPrincipal(projectIdentity, null);

                // Reset cookie for a sliding expiration
                formsAuthentication.SetAuthCookie(Context, ticket);
            }
        }

        private static void InitializeDependencyInjectionContainer()
        {
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(Container));
            DependencyResolver.SetResolver(new UnityDependencyResolver(Container));
        }

        private static void InitializeDatabase()
        {
            var databaseInitializer = ServiceLocator.Current.GetInstance<IDatabaseInitializer>();
            databaseInitializer.Initialize();
        }

        //public override string GetVaryByCustomString(HttpContext context, string custom)
        //{
        //    if (custom.Equals("lang"))
        //    {
        //        //return Thread.CurrentThread.CurrentUICulture.Name;
        //        return CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        //    }

        //    return base.GetVaryByCustomString(context, custom);
        //}

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            if (Context.Response.RedirectLocation != null && Context.Response.RedirectLocation.Contains("ReturnUrl"))
            {
                Context.Response.RedirectLocation = "/login";
            }

            using (DependencyResolver.Current as IDisposable) { }
        }

        protected void ErrorLog_Filtering(object sender, ExceptionFilterEventArgs e)
        {
            FilterError404(e);
        }

        protected void ErrorMail_Filtering(object sender, ExceptionFilterEventArgs e)
        {
            FilterError404(e);
        }

        private static void FilterError404(ExceptionFilterEventArgs e)
        {
            if (e.Exception.GetBaseException() is HttpException)
            {
                var exception = (HttpException)e.Exception.GetBaseException();
                if (exception.GetHttpCode() == 404)
                    e.Dismiss();
            }
        }

        #if !DEBUG

        protected void Application_Error(object sender, EventArgs e)
        {
            var httpException = Server.GetLastError().GetBaseException() as HttpException;

            Response.Clear();
            
            var routeData = new RouteData();
            routeData.Values.Add("controller", "Error");

            if (httpException == null)
            {
                routeData.Values.Add("action", "Index");
            }
            else
            {
                switch (httpException.GetHttpCode())
                {
                    case 404:
                        routeData.Values.Add("action", "Error404"); // Page not found
                        break;
                    case 500:
                        routeData.Values.Add("action", "Error500"); // Server error
                        break;
                    default:
                        routeData.Values.Add("action", "Index");
                        break;
                }
            }

            // Clear the error on server
            Server.ClearError();

            // Avoid IIS7 getting in the middle
            Response.TrySkipIisCustomErrors = true;
            IController errorController = new ErrorController();
            errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
        }

        #endif
    }
}