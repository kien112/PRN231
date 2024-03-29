﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ScoreManagementClient.Dtos.Common;
using ScoreManagementClient.Dtos.User;
using ScoreManagementClient.Dtos.User.Request;
using ScoreManagementClient.Dtos.User.Response;
using System.Net.Http.Headers;
using System.Text;

namespace ScoreManagementClient.Controllers
{
    public class LoginController : Controller
    {
        private string baseUrl;
        private readonly HttpClient client;
        public LoginController() 
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            baseUrl = "https://localhost:7068/api/Auth/login";
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginRequest request)
        {
            string jsonRequest = JsonConvert.SerializeObject(request);

            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(baseUrl, content);
            ResponseData<LoginResponse>? responseData;
            
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                responseData = JsonConvert.DeserializeObject<ResponseData<LoginResponse>>(jsonResponse);
                
                if (responseData != null && responseData.StatusCode == 200)
                {
                    HttpContext.Response.Cookies.Append("Token", responseData.Data.Token);
                    UserTiny user = new UserTiny
                    {
                        FullName = responseData.Data.FullName,
                        Role = responseData.Data.Role
                    };
                    HttpContext.Response.Cookies.Append("UserInfo", JsonConvert.SerializeObject(user));
                    return Redirect("/home");
                }
                else
                {
                    return View(responseData);
                }
            }
            else
            {
                return View();
            }

        }

        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("Token");
            HttpContext.Response.Cookies.Delete("UserInfo");
            return Redirect("/home");
        }

        [HttpGet("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpGet("ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }
    }
}
