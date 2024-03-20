using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ScoreManagementClient.Dtos.Common;
using ScoreManagementClient.Dtos.ScoreDto.Request;
using ScoreManagementClient.Dtos.ScoreDto.Response;
using System.Net.Http.Headers;
using System.Text;

namespace ScoreManagementClient.Controllers
{
    public class ScoresController : BaseController
    {
        public ScoresController()
        {
         
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("MyScore")]
        public IActionResult MyScore()
        {
            return View();
        }
    }
}
