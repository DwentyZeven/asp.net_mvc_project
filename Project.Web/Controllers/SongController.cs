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
    public class SongController : AuthorizedController
    {
        private const int DEFAULT_PAGE_SIZE = 20;

        private readonly ISongRepository _songRepository;

        private long _userId;

        [InjectionConstructor]
        public SongController(IUserRepository userRepository, ISongRepository songRepository) : base(userRepository)
        {
            _songRepository = songRepository;
        }

        public ActionResult Index(int? page)
        {
            return GetListView(page);
        }

        public ActionResult List(int? page)
        {
            return GetListView(page);
        }

        public ActionResult ByUser(long? userId, int? page)
        {
            if (userId == null)
                return RedirectToRoute("SongList");

            _userId = (long) userId;

            if (userId == Config.MainAdminId)
                return RedirectToRoute("SongIndex");

            ViewBag.User = userRepository.GetUser(userId);
            if (ViewBag.User == null)
                return RedirectToRoute("SongList");

            return GetListView(page);
        }

        public ActionResult Details(long? songId)
        {
            ViewBag.IsAjaxRequest = Request.IsAjaxRequest();

            var song = _songRepository.GetSongLocal(songId);

            if (song == null)
                return RedirectToRoute("SongIndex");            
            
            return View(song);
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
        public ActionResult Add(FormCollection formValues, Song model)
        {
            if ((formValues != null) && (formValues["Save"] != null) && (ModelState.IsValid))
            {
                _songRepository.CreateSong(CurrentUserId, model);
                return CurrentUserId == Config.MainAdminId ? RedirectToRoute("SongIndex") : RedirectToRoute("SongList");
            }

            return View();
        }

        private ActionResult GetListView(int? page)
        {
            ViewBag.IsAjaxRequest = Request.IsAjaxRequest();

            var currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            _songRepository.SetPaginationParams(currentPageIndex, DEFAULT_PAGE_SIZE);

            ReadOnlyCollection<Song> songs = null;
            switch (RouteData.Values["action"] as string)
            {
                case "Index":
                    songs = _songRepository.GetUserSongsLocal(Config.MainAdminId, true);
                    break;
                case "List":
                    songs = _songRepository.GetAllSongsLocal(Config.MainAdminId, true);
                    break;
                case "ByUser":
                    songs = _songRepository.GetUserSongsLocal(_userId, true);
                    break;
            }

            if (songs == null || songs.Count < 1)
            {
                if (Request.IsAjaxRequest())
                    return new EmptyResult();

                return GetEmptyListView(RouteData.Values["action"] as string);
            }

            return View(RouteData.Values["action"] as string, songs);
        }

        private ActionResult GetEmptyListView(string action)
        {
            switch (action)
            {
                case "Index":
                    ViewBag.Title = Properties.Resources.SongIndexTitle;
                    ViewBag.Subject = Properties.Resources.SongIndexEmptySubject;
                    ViewBag.Description = Properties.Resources.SongIndexEmptyDescription;
                    break;
                case "List":
                    ViewBag.Title = Properties.Resources.SongListTitle;
                    ViewBag.Subject = Properties.Resources.SongListEmptySubject;
                    ViewBag.Description = Properties.Resources.SongListEmptyDescription;
                    break;
                case "ByUser":
                    ViewBag.Title = Properties.Resources.SongByUserTitle + " " + ViewBag.User.Firstname + " " + ViewBag.User.Lastname;
                    if (User.Identity.IsAuthenticated && _userId == CurrentUserId)
                    {
                        ViewBag.Subject = Properties.Resources.SongByCurrentUserEmptySubject;
                        ViewBag.Description = Properties.Resources.SongByCurrentUserEmptyDescription;
                    }
                    else
                    {
                        ViewBag.Subject = Properties.Resources.SongByUserEmptySubject;
                        ViewBag.Description = Properties.Resources.SongByUserEmptyDescription;
                    }
                    break;
            }

            return View("EmptyList");
        }
    }
}
