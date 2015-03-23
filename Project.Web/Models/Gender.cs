using System.Collections.Generic;

namespace Project.Web.Models
{
    public static class Gender
    {
        public static List<ListItem> GenderList { get; set; }

        static Gender()
        {
            GenderList = new List<ListItem> {
                new ListItem { Id = 0, Text = "unknown"},
                new ListItem { Id = 1, Text = "female"},
                new ListItem { Id = 2, Text = "male"}
            };
        }
    }
}