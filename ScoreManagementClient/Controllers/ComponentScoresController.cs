using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ScoreManagementClient.Dtos.Common;
using ScoreManagementClient.Dtos.ComponentScoreDto;
using System.Net.Http.Headers;
using System.Text;

namespace ScoreManagementClient.Controllers
{
    public class ComponentScoresController : BaseController
    {
        private HttpClient client;
        public ComponentScoresController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            BaseUrl += "/ComponentScore";
        }

        public async Task<IActionResult> Index()
        {
            client = AddTokenToHeader(client);

            SearchComponentScores searchSubject = new SearchComponentScores();
            searchSubject.PageSize = 2;

            var content = new StringContent(JsonConvert.SerializeObject(searchSubject), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = await client.PostAsync(BaseUrl + "/search-component-score", content);

            if (httpResponse.IsSuccessStatusCode)
            {
                string jsonResponse = await httpResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<ResponseData<SearchComponentScores>>(jsonResponse);
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
                    return Redirect("/home");
                }
            }
            else
            {
                return Redirect("/home");
            }

        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Update(int id)
        {
            ViewBag.Id = id;
            return View();
        }
    }
}
