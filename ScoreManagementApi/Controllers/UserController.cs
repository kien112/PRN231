***REMOVED***using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScoreManagementApi.Core.Dtos.Common;
using ScoreManagementApi.Core.Dtos.User;
using ScoreManagementApi.Core.Dtos.User.Response;
using ScoreManagementApi.Services;

namespace ScoreManagementApi.Controllers
***REMOVED***
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    ***REMOVED***
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        ***REMOVED***
            _userService = userService;
***REMOVED***

        [HttpPost]
        [Route("get-by-id")]
        public async Task<ResponseData<UserResponse>> GetUserById(string Id)
        ***REMOVED***
            return await _userService.GetUserById(Id);
***REMOVED***

        [HttpPost]
        [Route("search-users")]
        public async Task<ResponseData<SearchList<UserResponse>>> SearchUsers(SearchUsers request)
        ***REMOVED***
            return await _userService.GetUsers(request);
***REMOVED***
***REMOVED***
***REMOVED***
