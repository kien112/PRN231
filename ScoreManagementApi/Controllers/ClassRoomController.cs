using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScoreManagementApi.Core.DbContext;
using ScoreManagementApi.Core.Dtos.ClassRoomDto;
using ScoreManagementApi.Core.Dtos.ClassRoomDto.Request;
using ScoreManagementApi.Core.Dtos.ClassRoomDto.Response;
using ScoreManagementApi.Core.Dtos.Common;
using ScoreManagementApi.Core.OtherObjects;
using ScoreManagementApi.Services;
using ScoreManagementApi.Utils;
using System.Net;

namespace ScoreManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassRoomController : ControllerBase
    {

        private readonly IClassService _classService;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public ClassRoomController(IClassService classService, IConfiguration configuration,
            ApplicationDbContext context)
        {
            _classService = classService;
            _configuration = configuration;
            _context = context;
        }

        [HttpPost]
        [Route("create-class")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<ResponseData<ClassResponse>> CreateClassRoom([FromHeader] string Authorization,
            [FromBody] CreateClassRequest request)
        {
            return await _classService.CreateClass(JWTUtil
                .GetUserFromToken(_configuration, _context, Authorization), request);
        }

        [HttpPut]
        [Route("update-class")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<ResponseData<ClassResponse>> UpdateClassRoom([FromHeader] string Authorization,
            [FromBody] UpdateClassRequest request)
        {
            return await _classService.UpdateClassRoom(JWTUtil
                .GetUserFromToken(_configuration, _context, Authorization), request);
        }

        [HttpGet]
        [Route("get-class-by-id/{id}")]
        [Authorize]
        public async Task<ResponseData<ClassResponse>> GetClassRoomById([FromHeader] string Authorization,
            int id)
        {
            return await _classService.GetClassRoomById(JWTUtil
                .GetUserFromToken(_configuration, _context, Authorization), id);
        }

        [HttpPost]
        [Route("search-class")]
        [Authorize]
        public async Task<ResponseData<SearchList<ClassResponse>>> SearchClassRoom([FromHeader] string Authorization,
            [FromBody] SearchClassRoom request)
        {
            return await _classService.SearchClassRoom(JWTUtil
                .GetUserFromToken(_configuration, _context, Authorization), request);
        }

        [HttpPost]
        [Route("cud-students-to-class")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<ResponseData<ClassResponse>> CUDStudentsToClassRoom([FromHeader] string Authorization,
            [FromBody] CUDStudentsToClass request)
        {
            return await _classService.CUDStudentsToClassRoom(JWTUtil
                .GetUserFromToken(_configuration, _context, Authorization), request);
        }
    }
}
