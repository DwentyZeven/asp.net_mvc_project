using System.Collections.Generic;

namespace Project.Web.Models
{
    public static class Role
    {
        public static List<ListItem> RoleList { get; set; }

        static Role()
        {
            RoleList = new List<ListItem> {
                new ListItem { Id = 1, Text = "admin"},
                new ListItem { Id = 2, Text = "user"}
            };
        }
    }
}