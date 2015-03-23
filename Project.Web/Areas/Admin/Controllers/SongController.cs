using System.Collections.ObjectModel;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Project.Interfaces;
using Project.Models;
using Project.Web.Controllers;
using Project.Web.Extensions.Attributes;
using Project.Web.Extensions.Pagination;

namespace Project.Web.Areas.Admin.Controllers
{
    [AdminAuthorize]
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

        public ActionResult ByUser(long userId, int? page)
        {
            _userId = userId;

            if (userId == Config.MainAdminId)
                return RedirectToRoute("AdminSongIndex");

            ViewBag.User = userRepository.GetUser(userId);
            if (ViewBag.User == null)
                return RedirectToRoute("AdminSongList");

            return GetListView(page);
        }

        public ActionResult ByDeletedUsers(int? page)
        {
            return GetListView(page);
        }

        private ActionResult GetListView(int? page)
        {
            var currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            _songRepository.SetPaginationParams(currentPageIndex, DEFAULT_PAGE_SIZE);

            var songCount = 0L;
            ReadOnlyCollection<Song> songs = null;
            switch (RouteData.Values["action"] as string)
            {
                case "Index":
                    songCount = _songRepository.CountUserSongs(Config.MainAdminId);
                    songs = _songRepository.GetUserSongs(Config.MainAdminId, true);
                    break;
                case "List":
                    songCount = _songRepository.CountAllSongs(Config.MainAdminId);
                    songs = _songRepository.GetAllSongs(Config.MainAdminId, true);
                    break;
                case "ByUser":
                    songCount = _songRepository.CountUserSongs(_userId);
                    songs = _songRepository.GetUserSongs(_userId, true);
                    break;
                case "ByDeletedUsers":
                    songCount = _songRepository.CountSongsByDeletedUsers();
                    songs = _songRepository.GetSongsByDeletedUsers(true);
                    break;
            }

            return View(RouteData.Values["action"] as string, songs.ToPagedList(currentPageIndex, DEFAULT_PAGE_SIZE, songCount));
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Add(FormCollection formValues, Song model)
        {
            if ((formValues != null) && (formValues["Save"] != null) && (ModelState.IsValid))
            {
                _songRepository.CreateSong(Config.MainAdminId, model);
                return RedirectToRoute("AdminSongIndex");
            }

            return View();
        }

        public ActionResult Edit(long songId)
        {
            ViewData["Referrer"] = (Request.UrlReferrer != null) ? Request.UrlReferrer.OriginalString : null;

            var song = _songRepository.GetSong(songId);
            if (song != null)
                return View(song);

            return Redirect(ViewData["Referrer"] as string);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection formValues, Song model)
        {
            if (formValues != null)
                ViewData["Referrer"] = formValues["Referrer"];

            if ((formValues != null) && (formValues["Save"] != null) && (ModelState.IsValid))
            {
                _songRepository.UpdateSong(model);
                return Redirect(formValues["Referrer"]);
            }

            return View();
        }

        [HttpPost]
        public JsonResult AjaxDelete(long songId)
        {
            _songRepository.DeleteSong(songId);
            return new JsonResult();
        }
    }
}
