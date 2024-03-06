using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScoreManagementApi.Core.DbContext;
using ScoreManagementApi.Core.Dtos.Common;
using ScoreManagementApi.Core.Dtos.ComponentScoreDto;
using ScoreManagementApi.Core.Dtos.ComponentScoreDto.Request;
using ScoreManagementApi.Core.Dtos.ComponentScoreDto.Response;
using ScoreManagementApi.Core.Dtos.SubjectDto;
using ScoreManagementApi.Core.OtherObjects;
using ScoreManagementApi.Services;
using ScoreManagementApi.Utils;

namespace ScoreManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComponentScoreController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IComponentScoreService _componentScoreService;

        public ComponentScoreController(ApplicationDbContext context, IConfiguration configuration,
            IComponentScoreService componentScoreService)
        {
            _context = context;
            _componentScoreService = componentScoreService;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("create-component-score")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<ResponseData<ComponentScoreResponse>> CreateComponentScore([FromHeader] string Authorization,
            [FromBody] CreateComponentScoreRequest request)
        {
            return await _componentScoreService.CreateComponentScore(JWTUtil.GetUserFromToken(_configuration, 
                _context, Authorization), request);
        }

        [HttpPost]
        [Route("search-component-score")]
        [Authorize]
        public async Task<ResponseData<SearchList<ComponentScoreResponse>>> SearchComponentScore([FromHeader] string Authorization,
            [FromBody] SearchComponentScores request)
        {
            return await _componentScoreService.SearchComponentScore(JWTUtil.GetUserFromToken(_configuration,
                _context, Authorization), request);
        }

        [HttpPut]
        [Route("update-component-score")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<ResponseData<ComponentScoreResponse>> UpdateComponentScore([FromHeader] string Authorization,
            [FromBody] UpdateComponentScoreRequest request)
        {
            return await _componentScoreService.UpdateComponentScore(JWTUtil.GetUserFromToken(_configuration,
                _context, Authorization), request);
        }

        [HttpGet]
        [Route("get-component-score-by-id/{id}")]
        [Authorize]
        public async Task<ResponseData<ComponentScoreResponse>> GetComponentScoreById([FromHeader] string Authorization,
            int id)
        {
            return await _componentScoreService.GetComponentScoreById(JWTUtil.GetUserFromToken(_configuration,
                _context, Authorization), id);
        }

        [HttpDelete]
        [Route("get-component-score-by-id/{id}")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<ResponseData<int?>> DeleteComponentScore([FromHeader] string Authorization,
            int id)
        {
            return await _componentScoreService.DeleteComponentScore(JWTUtil.GetUserFromToken(_configuration,
                _context, Authorization), id);
        }
    }
}
