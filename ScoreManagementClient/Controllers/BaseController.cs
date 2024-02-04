using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace ScoreManagementClient.Controllers
{
    public class BaseController : Controller
    {
        public string BaseUrl = "https://localhost:7068/api";
        private string? Token;

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
            return Token;
        }
    }
}
