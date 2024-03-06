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
        public async Task<ResponseData<SearchList<ScoreResponse>>> SearchScore([FromHeader] string Authorization,
            [FromBody] SearchScoreRequest request)
        {
            return await _scoreService.SearchScore(JWTUtil
                .GetUserFromToken(_configuration, _context, Authorization), request);
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
        public async Task<IActionResult> ExportScore([FromHeader] string Authorization, int? ClassId)
        {
            var responseData = await _scoreService.ExportScore(JWTUtil
            .GetUserFromToken(_configuration, _context, Authorization), ClassId);

            if(responseData.StatusCode == 200)
            {
                byte[] excelBytes = responseData.Data.Bytes;

                Response.Headers.Add("FileName", responseData.Data.FileName);

                return File(excelBytes,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        responseData.Data.FileName);
            }
            else
            {
                return BadRequest(responseData.Message);
            }

        }
    }
}
