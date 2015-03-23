using System.Web.Mvc;

namespace Project.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "AdminSongIndex",
                "admin",
                new { controller = "Song", action = "Index" },
                new[] { "Project.Web.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "AdminSongList",
                "admin/quotes",
                new { controller = "Song", action = "List" },
                new[] { "Project.Web.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "AdminSongByUser",
                "admin/quotes_by_user/{userId}",
                new { controller = "Song", action = "ByUser", userId = UrlParameter.Optional },
                new[] { "Project.Web.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "AdminSongByDeletedUsers",
                "admin/quotes_by_deleted_users",
                new { controller = "Song", action = "ByDeletedUsers" },
                new[] { "Project.Web.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "AdminSongAdd",
                "admin/add_quote",
                new { controller = "Song", action = "Add" },
                new[] { "Project.Web.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "AdminSongEdit",
                "admin/edit_quote/{songId}",
                new { controller = "Song", action = "Edit", songId = UrlParameter.Optional },
                new[] { "Project.Web.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "AdminSongAjaxDelete",
                "admin/delete_quote/{songId}",
                new { controller = "Song", action = "AjaxDelete", songId = UrlParameter.Optional },
                new[] { "Project.Web.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "AdminSongDeleteWithoutUser",
                "admin/delete_quotes_without_users",
                new { controller = "Song", action = "DeleteWithoutUser" },
                new[] { "Project.Web.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "AdminUserList",
                "admin/users",
                new { controller = "User", action = "List" },
                new[] { "Project.Web.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "AdminUserEdit",
                "admin/edit_user/{userId}",
                new { controller = "User", action = "Edit", userId = UrlParameter.Optional },
                new[] { "Project.Web.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "AdminUserAjaxDelete",
                "admin/delete_user/{userId}",
                new { controller = "User", action = "AjaxDelete", userId = UrlParameter.Optional },
                new[] { "Project.Web.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "AdminElmahIndex",
                "admin/elmah/{type}",
                new { controller = "Elmah", action = "Index", type = UrlParameter.Optional },
                new[] { "Project.Web.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "AdminElmahDetail",
                "admin/elmah/detail/{type}",
                new { controller = "Elmah", action = "Detail", type = UrlParameter.Optional, id = UrlParameter.Optional },
                new[] { "Project.Web.Areas.Admin.Controllers" }
            );
        }
    }
}