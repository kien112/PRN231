using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ScoreManagementClient.Dtos.User;
using System.Net.Http.Headers;

namespace ScoreManagementClient.Controllers
{
    public class BaseController : Controller
    {
        public string BaseUrl = "https://localhost:7068/api";
        private string? Token;
        private UserTiny? UserInfo;

        public HttpClient AddTokenToHeader(HttpClient client)
        {
            Token = HttpContext?.Request?.Cookies["Token"];

            if (!string.IsNullOrEmpty(Token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            }

            return client;
        }

        public string? GetToken()
        {
            Token = HttpContext?.Request?.Cookies["Token"];
            return Token;
        }

        public UserTiny? GetUserInfo()
        {
            string? jsonUser = HttpContext?.Request?.Cookies["UserInfo"];
            
            if(String.IsNullOrEmpty(jsonUser))
                return null;

            UserInfo = JsonConvert.DeserializeObject<UserTiny>(jsonUser);

            return UserInfo;
        }

        public bool CanAccess(List<string> roles)
        {
            if(GetToken() == null)
                return false;

            if(GetUserInfo() == null || UserInfo == null) 
                return false;

            return roles.Contains(UserInfo.Role);
        }
    }
}
