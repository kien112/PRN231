using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScoreManagementApi.Core.DbContext;
using ScoreManagementApi.Core.Dtos.Common;
using ScoreManagementApi.Core.Dtos.SubjectDto;
using ScoreManagementApi.Core.Dtos.SubjectDto.Request;
using ScoreManagementApi.Core.Dtos.SubjectDto.Response;
using ScoreManagementApi.Core.OtherObjects;
using ScoreManagementApi.Services;
using ScoreManagementApi.Utils;

namespace ScoreManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {

        private readonly ISubjectService _subjectService;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public SubjectController(ISubjectService subjectService, IConfiguration configuration,
            ApplicationDbContext context)
        {
            _subjectService = subjectService;
            _configuration = configuration;
            _context = context;
        }

        [HttpPost]
        [Route("create-subject")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<ResponseData<SubjectResponse>> CreateSubject([FromHeader] string Authorization, 
            [FromBody] CreateSubjectRequest request)
        {
            return await _subjectService.CreateSubject(JWTUtil
                .GetUserFromToken(_configuration, _context, Authorization) , request);
        }

        [HttpPut]
        [Route("update-subject")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<ResponseData<SubjectResponse>> UpdateSubject([FromHeader] string Authorization,
            [FromBody] UpdateSubjectRequest request)
        {
            return await _subjectService.UpdateSubject(JWTUtil
                .GetUserFromToken(_configuration, _context, Authorization), request);
        }

        [HttpGet]
        [Route("get-subject-by-id/{id}")]
        [Authorize]
        public async Task<ResponseData<SubjectResponse>> GetSubjectById([FromHeader] string Authorization,
            int? id)
        {
            return await _subjectService.GetSubjectById(JWTUtil
                .GetUserFromToken(_configuration, _context, Authorization), id);
        }

        [HttpPost]
        [Route("search-subjects")]
        [Authorize]
        public async Task<ResponseData<SearchList<SubjectResponse>>> SearchSubjects([FromHeader] string Authorization,
            SearchSubject request)
        {
            return await _subjectService.SearchSubjects(JWTUtil
                .GetUserFromToken(_configuration, _context, Authorization), request);
        }
    }
}
