using Newtonsoft.Json;
using ScoreManagementClient.Dtos.User;
using ScoreManagementClient.OtherObjects;

namespace ScoreManagementClient.Extensions
{
    public class TokenCheckMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenCheckMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path;

            if (path.StartsWithSegments("/subjects") || path.StartsWithSegments("/users") || path.StartsWithSegments("/classrooms")
                || path.StartsWithSegments("/componentscores") || path.StartsWithSegments("/scores") || path.StartsWithSegments("/myscore"))
            {
                var token = context.Request.Cookies["Token"];
                var userInfo = context.Request.Cookies["UserInfo"];
                UserTiny? user = null;
                if(userInfo != null)
                {
                    user = JsonConvert.DeserializeObject<UserTiny>(userInfo);
                }

                if (string.IsNullOrEmpty(token) || user == null)
                {
                    context.Response.Redirect("/login");
                    return;
                }

                var pathSegments = path.Value.Split('/');

                if(pathSegments != null && !user.Role.Equals(StaticUserRoles.ADMIN) 
                    && (pathSegments.Contains("create") || pathSegments.Contains("update") || pathSegments.Contains("students")))
                {
                    context.Response.Redirect("/PermissionDenied");
                    return;
                }
            }

            await _next(context);
        }
    }

}
