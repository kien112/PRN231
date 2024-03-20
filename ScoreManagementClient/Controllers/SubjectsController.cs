using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ScoreManagementClient.Dtos.Common;
using ScoreManagementClient.Dtos.SubjectDto;
using ScoreManagementClient.Dtos.SubjectDto.Response;
using ScoreManagementClient.OtherObjects;
using ScoreManagementClient.Utills;
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
            searchSubject.PageSize = 2;

            var content = new StringContent(JsonConvert.SerializeObject(searchSubject), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = await client.PostAsync(BaseUrl + "/search-subjects", content);

            if(httpResponse.IsSuccessStatusCode)
            {
                string jsonResponse = await httpResponse.Content.ReadAsStringAsync();
                var response  = JsonConvert.DeserializeObject<ResponseData<SearchSubject>>(jsonResponse);
                if(response != null && response.StatusCode == 200)
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
            if(CanAccess(new List<string> { StaticUserRoles.ADMIN }))
            {
                return View();
            }
            return Redirect("/PermissionDenied");
        }

        public async Task<IActionResult> Update(int id)
        {
            client = AddTokenToHeader(client);

            HttpResponseMessage httpResponse = await client.GetAsync(BaseUrl + $"/get-subject-by-id/{id}");

            if(httpResponse.IsSuccessStatusCode)
            {
                string jsonResponse = await httpResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<ResponseData<SubjectResponse>>(jsonResponse);
                if (response != null && response.StatusCode == 200)
                {
                    return View(response);
                }
            }

            return Redirect("/NotFound");
        }

    }
}
