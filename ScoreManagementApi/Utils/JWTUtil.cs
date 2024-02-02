***REMOVED***using Microsoft.IdentityModel.Tokens;
using ScoreManagementApi.Core.DbContext;
using ScoreManagementApi.Core.Dtos.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ScoreManagementApi.Utils
***REMOVED***
    public static class JWTUtil
    ***REMOVED***
        public static UserTiny? GetUserFromToken(IConfiguration configuration, ApplicationDbContext context, string jwtToken)
        ***REMOVED***
            if(jwtToken.StartsWith("Bearer "))
                jwtToken = jwtToken.Substring(7);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters
            ***REMOVED***
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = configuration["JWT:ValidateIssuer"],
                ValidAudience = configuration["JWT:ValidateAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
***REMOVED***

            ClaimsPrincipal principal = tokenHandler.ValidateToken(jwtToken, tokenValidationParameters, out SecurityToken validatedToken);

            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
            var userRole = principal.FindFirst(ClaimTypes.Role);
            UserTiny? userTiny = null;
            
            if (userIdClaim != null)
            ***REMOVED***
                var user = context.Users.Find(userIdClaim.Value);
                if (user != null && userRole != null)
                    userTiny = new UserTiny
                    ***REMOVED***
                        Id = user.Id,
                        FullName = user.FullName,
                        Role = userRole.Value
        ***REMOVED***
    ***REMOVED***

            return userTiny;
***REMOVED***
***REMOVED***
***REMOVED***
