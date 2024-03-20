using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScoreManagementApi.Core.Dtos.Common;
using ScoreManagementApi.Core.Dtos.User;
using ScoreManagementApi.Core.Dtos.User.Response;
using ScoreManagementApi.Core.OtherObjects;
using ScoreManagementApi.Services;

namespace ScoreManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("get-by-id/{Id}")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<ResponseData<UserResponse>> GetUserById(string Id)
        {
            return await _userService.GetUserById(Id);
        }

        [HttpPost]
        [Route("search-users")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<ResponseData<SearchList<UserResponse>>> SearchUsers(SearchUsers request)
        {
            return await _userService.GetUsers(request);
        }
    }
}
