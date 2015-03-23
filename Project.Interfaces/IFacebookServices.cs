using Project.Models;

namespace Project.Interfaces
{
    public interface IFacebookServices
    {
        string GetAccessToken(string request);

        User GetUserInfo(string accessToken);
    }
}
