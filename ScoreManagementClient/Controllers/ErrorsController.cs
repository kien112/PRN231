using Microsoft.AspNetCore.Mvc;

namespace ScoreManagementClient.Controllers
{
    public class ErrorsController : Controller
    {
        [HttpGet("NotFound")]
        public IActionResult NotFoundPage()
        {
            return View();
        }

        [HttpGet("BadRequest")]
        public IActionResult BadRequestPage()
        {
            return View();
        }

        [HttpGet("PermissionDenied")]
        public IActionResult PemissionDenied()
        {
            return View();
        }
    }
}
