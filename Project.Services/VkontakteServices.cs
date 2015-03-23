using System;
using System.IO;
using System.Net;
using System.Text;
using Project.Interfaces;
using Project.Models;
using Project.Services.Properties;
using Newtonsoft.Json.Linq;

namespace Project.Services
{
    public class VkontakteServices : IVkontakteServices
    {
        private const string VKONTAKTE_URL = "http://vkontakte.ru/";

        private const string GET_USER_INFO = "https://api.vk.com/method/getProfiles?uid={0}&fields=photo_medium,domain,sex,timezone&access_token={1}";

        private long _userId;
        
        public string GetAccessToken(string request)
        {
            try
            {
                var response = GetHttpResponse(request);

                if (response != null)
                {
                    JObject jsonObject = JObject.Parse(response);
                    _userId = jsonObject.Value<long>("user_id");
                    return (string) jsonObject["access_token"];
                }
                return null;
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToGetVkontakteAccessTokenExceptionMessage, e);
            }
        }

        public User GetUserInfo(string accessToken)
        {
            try
            {
                var request = string.Format(GET_USER_INFO, _userId, accessToken);
                var response = GetHttpResponse(request);

                if (response != null)
                {
                    var jsonObject = (JObject) JObject.Parse(response).SelectToken("response[0]");

                    return new User
                    {
                        VkontakteId = _userId,
                        Firstname = (string) jsonObject["first_name"],
                        Lastname = (string) jsonObject["last_name"],
                        Gender = jsonObject.Value<short>("sex"),
                        Link = VKONTAKTE_URL + jsonObject["domain"],
                        PhotoLink = (string) jsonObject["photo_medium"]
                    };
                }
                return null;
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToGetVkontakteUserInfoExceptionMessage, e);
            }
        }

        private static string GetHttpResponse(string request)
        {
            try
            {
                var httpRequest = (HttpWebRequest) WebRequest.Create(request);
                var httpResponse = (HttpWebResponse) httpRequest.GetResponse();
                var httpResponseStream = httpResponse.GetResponseStream();

                return httpResponseStream != null ? new StreamReader(httpResponseStream, Encoding.UTF8).ReadToEnd() : null;
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToGetHttpResponseExceptionMessage, e);
            }
        }
    }
}
