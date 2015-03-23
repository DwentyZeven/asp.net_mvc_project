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
    public class FacebookServices : IFacebookServices
    {
        private const string GET_USER_INFO = "https://graph.facebook.com/me?access_token={0}";

        private const string PROFILE_PHOTO_URL = "http://graph.facebook.com/{0}/picture?type=normal";

        public string GetAccessToken(string request)
        {
            try
            {
                var response = GetHttpResponse(request);

                if (response != null)
                {
                    var pairResponse = response.Split('&');
                    var accessToken = pairResponse[0].Split('=')[1];
                    return accessToken;
                }
                return null;
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToGetFacebookAccessTokenExceptionMessage, e);
            }
        }

        public User GetUserInfo(string accessToken)
        {
            try
            {
                var request = string.Format(GET_USER_INFO, accessToken);
                var response = GetHttpResponse(request);

                if (response != null)
                {
                    JObject jsonObject = JObject.Parse(response);

                    return new User
                    {
                        FacebookId = jsonObject.Value<long>("id"),
                        Firstname = (string) jsonObject["first_name"],
                        Lastname = (string) jsonObject["last_name"],
                        Gender = (short) (((string) jsonObject["gender"] == "female") ? 1 : ((string) jsonObject["gender"] == "male") ? 2 : 0),
                        Link = (string) jsonObject["link"],
                        PhotoLink = string.Format(PROFILE_PHOTO_URL, jsonObject.Value<long>("id")),
                        Email = (string) jsonObject["email"]
                    };
                }
                return null;
            }
            catch (InvalidOperationException e)
            {
                throw new BusinessServiceException(Resources.UnableToGetFacebookUserInfoExceptionMessage, e);
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
