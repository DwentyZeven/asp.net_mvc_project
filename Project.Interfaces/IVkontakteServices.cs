using Project.Models;

namespace Project.Interfaces
{
    public interface IVkontakteServices
    {
        string GetAccessToken(string request);

        User GetUserInfo(string accessToken);
    }
}
