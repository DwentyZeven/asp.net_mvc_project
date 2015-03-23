using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Project.Interfaces;
using Project.Models;
using Project.Web.Controllers;
using Project.Web.Extensions.Attributes;
using Project.Web.Extensions.Pagination;
using Project.Web.Models;

namespace Project.Web.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class UserController : AuthorizedController
    {
        private const int DEFAULT_PAGE_SIZE = 20;

        [InjectionConstructor]
        public UserController(IUserRepository userRepository) : base(userRepository)
        {
        }

        public ActionResult List(int? page)
        {
            var currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            userRepository.SetPaginationParams(currentPageIndex, DEFAULT_PAGE_SIZE);

            var userCount = userRepository.CountAllUsers();
            var users = userRepository.GetAllUsers(true).ToPagedList(currentPageIndex, DEFAULT_PAGE_SIZE, userCount);
            return View(users);
        }

        public ActionResult Edit(long userId)
        {
            ViewData["Referrer"] = (Request.UrlReferrer != null) ? Request.UrlReferrer.OriginalString : Request.RawUrl;

            var user = userRepository.GetUser(userId);
            if (user != null)
            {
                ViewData["GenderList"] = new SelectList(Gender.GenderList, "Id", "Text", user.Gender);
                ViewData["RoleList"] = new SelectList(Role.RoleList, "Id", "Text", user.Role);
                return View(user);
            }            

            return Redirect(ViewData["Referrer"] as string);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection formValues, User model)
        {
            if (formValues != null)
            {
                ViewData["Referrer"] = formValues["Referrer"];
                ViewData["GenderList"] = new SelectList(Gender.GenderList, "Id", "Text", formValues["Gender"]);
                ViewData["RoleList"] = new SelectList(Role.RoleList, "Id", "Text", formValues["Role"]);
            }

            if ((formValues != null) && (formValues["Save"] != null) && (ModelState.IsValid))
            {
                userRepository.UpdateUser(model);
                return Redirect(ViewData["Referrer"] as string);
            }

            return View();
        }

        [HttpPost]
        public JsonResult AjaxDelete(long userId)
        {
            if (userId == Config.MainAdminId)
                return new JsonResult();

            userRepository.DeleteUser(userId);
            return new JsonResult();
        }
    }
}
