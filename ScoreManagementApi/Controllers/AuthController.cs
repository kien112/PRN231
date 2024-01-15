***REMOVED***using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ScoreManagementApi.Core.Entities;
using ScoreManagementApi.Core.OtherObjects;
using System.Data;

namespace ScoreManagementApi.Controllers
***REMOVED***
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    ***REMOVED***
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
***REMOVED***

        public AuthController(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        ***REMOVED***
            _userManager = userManager;
            _roleManager = roleManager;
***REMOVED***
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
***REMOVED***
***REMOVED***
