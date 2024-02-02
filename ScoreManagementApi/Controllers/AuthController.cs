***REMOVED***using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ScoreManagementApi.Core.Dtos.Common;
using ScoreManagementApi.Core.Dtos.User.Request;
using ScoreManagementApi.Core.Dtos.User.Response;
using ScoreManagementApi.Core.Dtos.User;
using ScoreManagementApi.Core.Entities;
using ScoreManagementApi.Core.OtherObjects;
using System.Data;
using ScoreManagementApi.Services;

namespace ScoreManagementApi.Controllers
***REMOVED***
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    ***REMOVED***
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
***REMOVED***
        private readonly IUserService _userService;

        public AuthController(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager, IConfiguration configuration, IUserService userService)
        ***REMOVED***
            _userManager = userManager;
            _roleManager = roleManager;
***REMOVED***
            _userService = userService;
***REMOVED***

        [HttpPost]
        [Route("seed-roles")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> SeedRoles()
        ***REMOVED***
            bool isOwnerRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.TEACHER);
            bool isUserRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.STUDENT);
            bool isAdminRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.ADMIN);

            if (isOwnerRoleExists && isUserRoleExists && isAdminRoleExists)
            ***REMOVED***
                return Ok("role is already seeding done");
    ***REMOVED***

            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.STUDENT));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.ADMIN));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.TEACHER));

            return Ok("role seeding done successfully");
***REMOVED***

        [HttpPost]
        [Route("create-user")]
        //[Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<ResponseData<UserResponse>> CreateUser([FromBody] CreateUserRequest request)
        ***REMOVED***
            return await _userService.CreateUser(request);
***REMOVED***

        [HttpPut]
        [Route("update-user")]
        //[Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<ResponseData<UserResponse>> UpdateUser([FromBody] UpdateUserRequest request)
        ***REMOVED***
            return await _userService.UpdateUser(request);
***REMOVED***

        [HttpPost]
        [Route("login")]
        public async Task<ResponseData<LoginResponse>> Login([FromBody] LoginRequest request)
        ***REMOVED***
            //request.UserNameOrEmail = "admin";
            //request.Password = "123456";
            return await _userService.Login(request);
***REMOVED***

        [HttpPut]
        [Route("update-user-role")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<ResponseData<UpdateUserRole>> UpdateUserRole(UpdateUserRole request)
        ***REMOVED***
            return await _userService.UpdateUserRole(request);
***REMOVED***

        [HttpPut]
        [Route("forgot-password")]
        public async Task<ResponseData<string>> ForgotPassword([FromBody] ForgotPasswordRequest request)
        ***REMOVED***
            return await _userService.ForgotPassword(request);
***REMOVED***
***REMOVED***
***REMOVED***
