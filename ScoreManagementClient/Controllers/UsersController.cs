using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ScoreManagementClient.Dtos.Common;
using ScoreManagementClient.Dtos.User;
using ScoreManagementClient.Dtos.User.Response;
using System.Net.Http.Headers;
using System.Text;

namespace ScoreManagementClient.Controllers
{
    public class UsersController : BaseController
    {
        private HttpClient client;
        public UsersController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            BaseUrl += "/User";
        }
        public async Task<IActionResult> Index()
        {
            client = AddTokenToHeader(client);

            SearchUsers searchUser = new SearchUsers();
            searchUser.PageSize = 2;

            var content = new StringContent(JsonConvert.SerializeObject(searchUser), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = await client.PostAsync(BaseUrl + "/search-users", content);

            if (httpResponse.IsSuccessStatusCode)
            {
                string jsonResponse = await httpResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<ResponseData<SearchList<UserResponse>>>(jsonResponse);
                if (response != null && response.StatusCode == 200)
                {
                    int? numberOfPage = response.Data.TotalElements % response.Data.PageSize == 0
                        ? response.Data.TotalElements / response.Data.PageSize
                        : 1 + response.Data.TotalElements / response.Data.PageSize;
                    ViewBag.NumberOfPage = numberOfPage;
                    return View(response);
                }
                else
                {
                    return Redirect("/BadRequest");
                }
            }
            else
            {
                return Redirect("/BadRequest");
            }

        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Update(string id)
        {
            ViewBag.Id = id;
            return View();
        }
    }
}
