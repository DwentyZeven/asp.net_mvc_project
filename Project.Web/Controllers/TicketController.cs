using System.Collections.ObjectModel;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Project.Interfaces;
using Project.Models;
using Project.Web.Extensions.Attributes;

namespace Project.Web.Controllers
{
    [CompressFilter(Order = 3)]
    [CacheFilter(Duration = 60, Order = 2)]
    [OutputCache(Duration = 60, VaryByParam = "none", Order = 1)]
    public class TicketController : AuthorizedController
    {
        private const int DEFAULT_PAGE_SIZE = 20;

        private readonly ITicketRepository _ticketRepository;

        private long _userId;

        [InjectionConstructor]
        public TicketController(IUserRepository userRepository, ITicketRepository ticketRepository) : base(userRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public ActionResult List(int? page)
        {
            return GetListView(page);
        }

        public ActionResult ByUser(long? userId, int? page)
        {
            if (userId == null)
                return RedirectToRoute("TicketList");

            _userId = (long) userId;

            ViewBag.User = userRepository.GetUser(userId);
            if (ViewBag.User == null)
                return RedirectToRoute("TicketList");

            return GetListView(page);
        }

        public ActionResult Details(long? ticketId)
        {
            ViewBag.IsAjaxRequest = Request.IsAjaxRequest();

            var ticket = _ticketRepository.GetTicketLocal(ticketId);

            if (ticket == null)
                return RedirectToRoute("TicketList");            
            
            return View(ticket);
        }

        [UserAuthorize]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [UserAuthorize]
        public ActionResult Add(FormCollection formValues, Ticket model)
        {
            if ((formValues != null) && (formValues["Save"] != null) && (ModelState.IsValid))
            {
                _ticketRepository.CreateTicket(CurrentUserId, model);
                return CurrentUserId == Config.MainAdminId ? RedirectToRoute("TicketIndex") : RedirectToRoute("TicketList");
            }

            return View();
        }

        private ActionResult GetListView(int? page)
        {
            ViewBag.IsAjaxRequest = Request.IsAjaxRequest();

            var currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            _ticketRepository.SetPaginationParams(currentPageIndex, DEFAULT_PAGE_SIZE);

            ReadOnlyCollection<Ticket> tickets = null;
            switch (RouteData.Values["action"] as string)
            {
                case "List":
                    tickets = _ticketRepository.GetAllTicketsLocal(Config.MainAdminId, true);
                    break;
                case "ByUser":
                    tickets = _ticketRepository.GetUserTicketsLocal(_userId, true);
                    break;
            }

            if (tickets == null || tickets.Count < 1)
            {
                if (Request.IsAjaxRequest())
                    return new EmptyResult();

                return GetEmptyListView(RouteData.Values["action"] as string);
            }

            return View(RouteData.Values["action"] as string, tickets);
        }

        private ActionResult GetEmptyListView(string action)
        {
            switch (action)
            {
                case "List":
                    ViewBag.Title = Properties.Resources.TicketListTitle;
                    ViewBag.Subject = Properties.Resources.TicketListEmptySubject;
                    ViewBag.Description = Properties.Resources.TicketListEmptyDescription;
                    break;
                case "ByUser":
                    ViewBag.Title = Properties.Resources.TicketByUserTitle + " " + ViewBag.User.Firstname + " " + ViewBag.User.Lastname;
                    if (User.Identity.IsAuthenticated && _userId == CurrentUserId)
                    {
                        ViewBag.Subject = Properties.Resources.TicketByCurrentUserEmptySubject;
                        ViewBag.Description = Properties.Resources.TicketByCurrentUserEmptyDescription;
                    }
                    else
                    {
                        ViewBag.Subject = Properties.Resources.TicketByUserEmptySubject;
                        ViewBag.Description = Properties.Resources.TicketByUserEmptyDescription;
                    }
                    break;
            }

            return View("EmptyList");
        }
    }
}
