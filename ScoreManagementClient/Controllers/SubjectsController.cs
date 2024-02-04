using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ScoreManagementClient.Dtos.SubjectDto;
using System.Net.Http.Headers;
using System.Text;

namespace ScoreManagementClient.Controllers
{
    public class SubjectsController : BaseController
    {
        private HttpClient client;
        public SubjectsController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            BaseUrl += "/Subject";
        }
        public async Task<IActionResult> Index()
        {
            client = AddTokenToHeader(client);

            SearchSubject searchSubject = new SearchSubject();

            var content = new StringContent(JsonConvert.SerializeObject(searchSubject), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = await client.PostAsync(BaseUrl + "/search-subjects", content);

            if(httpResponse.IsSuccessStatusCode)
            {

            }
            else
            {

            }

            return View();
        }
    }
}
