using System;
using System.Configuration;

namespace Project.Web
{
    public static class Config
    {
        public static short MainAdminId
        {
            get { return Convert.ToInt16(ConfigurationManager.AppSettings["mainAdminId"]); }
        }

        public static string ProjectName
        {
            get { return ConfigurationManager.AppSettings["projectName"]; }
        }
    }
}