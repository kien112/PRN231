***REMOVED***using Microsoft.AspNetCore.Authorization;
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
***REMOVED***
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    ***REMOVED***

        private readonly ISubjectService _subjectService;
***REMOVED***
        private readonly ApplicationDbContext _context;

        public SubjectController(ISubjectService subjectService, IConfiguration configuration,
            ApplicationDbContext context)
        ***REMOVED***
            _subjectService = subjectService;
***REMOVED***
            _context = context;
***REMOVED***

        [HttpPost]
        [Route("create-subject")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<ResponseData<SubjectResponse>> CreateSubject([FromHeader] string Authorization, 
            [FromBody] CreateSubjectRequest request)
        ***REMOVED***
            return await _subjectService.CreateSubject(JWTUtil
                .GetUserFromToken(_configuration, _context, Authorization) , request);
***REMOVED***

        [HttpPut]
        [Route("update-subject")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<ResponseData<SubjectResponse>> UpdateSubject([FromHeader] string Authorization,
            [FromBody] UpdateSubjectRequest request)
        ***REMOVED***
            return await _subjectService.UpdateSubject(JWTUtil
                .GetUserFromToken(_configuration, _context, Authorization), request);
***REMOVED***

        [HttpGet]
        [Route("get-subject-by-id/***REMOVED***id***REMOVED***")]
        [Authorize]
        public async Task<ResponseData<SubjectResponse>> GetSubjectById([FromHeader] string Authorization,
            int? id)
        ***REMOVED***
            return await _subjectService.GetSubjectById(JWTUtil
                .GetUserFromToken(_configuration, _context, Authorization), id);
***REMOVED***

        [HttpPost]
        [Route("search-subjects")]
        [Authorize]
        public async Task<ResponseData<SearchList<SubjectResponse>>> SearchSubjects([FromHeader] string Authorization,
            SearchSubject request)
        ***REMOVED***
            return await _subjectService.SearchSubjects(JWTUtil
                .GetUserFromToken(_configuration, _context, Authorization), request);
***REMOVED***
***REMOVED***
***REMOVED***
