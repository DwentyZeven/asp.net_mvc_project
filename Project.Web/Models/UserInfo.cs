using System.IO;
using System.Xml.Serialization;

namespace Project.Web.Models
{
    public class UserInfo
    {
        public long UserId { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }
        
        public string Username { get; set; }

        public string Link { get; set; }

        public short Role { get; set; }

        public bool IsWarned { get; set; }

        public bool IsBanned { get; set; }

        public override string ToString()
        {
            var serializer = new XmlSerializer(typeof (UserInfo));
            using (var stream = new StringWriter())
            {
                serializer.Serialize(stream, this);
                return stream.ToString();
            }
        }

        public static UserInfo FromString(string userContextData)
        {
            var serializer = new XmlSerializer(typeof(UserInfo));
            using (var stream = new StringReader(userContextData))
            {
                return serializer.Deserialize(stream) as UserInfo;
            }
        }
    }
}