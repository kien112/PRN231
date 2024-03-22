using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using ScoreManagementApi.Core.DbContext;
using ScoreManagementApi.Core.Dtos.Common;
using ScoreManagementApi.Core.Dtos.ScoreDto.Request;
using ScoreManagementApi.Core.Dtos.ScoreDto.Response;
using ScoreManagementApi.Core.OtherObjects;
using ScoreManagementApi.Services;
using ScoreManagementApi.Utils;
using System.Net;
using System.Net.Http.Headers;

namespace ScoreManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScoresController : ControllerBase
    {

        private readonly IScoreService _scoreService;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public ScoresController(IScoreService scoreService, IConfiguration configuration,
            ApplicationDbContext context)
        {
            _scoreService = scoreService;
            _configuration = configuration;
            _context = context;
        }


        [HttpPost]
        [Route("search-scores")]
        [Authorize(Roles = $"{StaticUserRoles.ADMIN}, {StaticUserRoles.TEACHER}")]
        public async Task<ResponseData<ScoreResponse>> SearchScore([FromHeader] string Authorization,
            [FromBody] SearchScoreRequest request)
        {
            return await _scoreService.SearchScore(JWTUtil
                .GetUserFromToken(_configuration, _context, Authorization), request);
        }

        [HttpGet]
        [Route("search-student-score/{subjectId}")]
        [Authorize(Roles = $"{StaticUserRoles.STUDENT}")]
        public async Task<ResponseData<List<StudentScoreResponse>>> SearchStudentScore([FromHeader] string Authorization,
             int subjectId)
        {
            return await _scoreService.SearchStudentScore(JWTUtil
                .GetUserFromToken(_configuration, _context, Authorization), subjectId);
        }

        [HttpGet]
        [Route("get-top-score/{subjectId}/{top}")]
        [Authorize]
        public async Task<ResponseData<TopScoreResponse>> GetTopScore([FromHeader] string Authorization,
             int subjectId, int top)
        {
            return await _scoreService.GetTopScore(JWTUtil
                .GetUserFromToken(_configuration, _context, Authorization), subjectId, top);
        }

        [HttpPost]
        [Route("cud-scores")]
        [Authorize(Roles = $"{StaticUserRoles.ADMIN}, {StaticUserRoles.TEACHER}")]
        public async Task<ResponseData<ScoreResponse>> CUDScore([FromHeader] string Authorization,
            [FromBody] List<CUDScoreRequest> request)
        {
            return await _scoreService.CUDScore(JWTUtil
                .GetUserFromToken(_configuration, _context, Authorization), request);
        }

        [HttpPost]
        [Route("import-score")]
        [Authorize(Roles = $"{StaticUserRoles.ADMIN}, {StaticUserRoles.TEACHER}")]
        public async Task<ResponseData<ScoreResponse>> ImportScore([FromHeader] string Authorization,
            [FromForm] ImportScoresRequest request)
        {
            return await _scoreService.ImportScore(JWTUtil
                .GetUserFromToken(_configuration, _context, Authorization), request);
        }

        [HttpGet]
        [Route("export-score/{ClassId}")]
        [Authorize(Roles = $"{StaticUserRoles.ADMIN}, {StaticUserRoles.TEACHER}")]
        public async Task<IActionResult> ExportScore([FromHeader] string Authorization, int? ClassId, bool isSwagger)
        {
            var responseData = await _scoreService.ExportScore(JWTUtil
            .GetUserFromToken(_configuration, _context, Authorization), ClassId);

            if(responseData.StatusCode == 200)
            {
                byte[] excelBytes = responseData.Data.Bytes;

                Response.Headers.Add("FileName", responseData.Data.FileName);

                return isSwagger ? File(excelBytes,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        responseData.Data.FileName) : Ok(excelBytes);
            }
            else
            {
                return BadRequest(responseData.Message);
            }

        }
    }
}
