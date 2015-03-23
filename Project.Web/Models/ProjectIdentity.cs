using System;
using System.Security.Principal;
using System.Web.Security;

namespace Project.Web.Models
{
    [Serializable]
    public class ProjectIdentity : MarshalByRefObject, IIdentity
    {
        public string Name { get; private set; }

        public string AuthenticationType
        {
            get { return Config.ProjectName; }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }

        public long UserId { get; private set; }

        public string Firstname { get; private set; }

        public string Lastname { get; private set; }

        public string Username { get; private set; }

        public string Link { get; private set; }

        public short Role { get; private set; }

        public bool IsWarned { get; private set; }

        public bool IsBanned { get; private set; }

        public ProjectIdentity(string name, long userId, string firstName, string lastName, string userName, string link, short role, bool isWarned, bool isBanned)
        {
            Name = name;
            UserId = userId;
            Firstname = firstName;
            Lastname = lastName;
            Username = userName;
            Link = link;
            Role = role;
            IsWarned = isWarned;
            IsBanned = isBanned;
        }

        public ProjectIdentity(string name, UserInfo userInfo) : this(name, userInfo.UserId, userInfo.Firstname, userInfo.Lastname, userInfo.Username, userInfo.Link, userInfo.Role, userInfo.IsWarned, userInfo.IsBanned)
        {
            if (userInfo == null) throw new ArgumentNullException("userInfo");
        }

        public ProjectIdentity(FormsAuthenticationTicket ticket) : this(ticket.Name, UserInfo.FromString(ticket.UserData))
        {
            if (ticket == null) throw new ArgumentNullException("ticket");
        }
    }
}