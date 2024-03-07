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

            if (path.StartsWithSegments("/subjects") || path.StartsWithSegments("/user") || path.StartsWithSegments("/classrooms"))
            {
                var token = context.Request.Cookies["Token"];

                if (string.IsNullOrEmpty(token))
                {
                    context.Response.Redirect("/login");
                    return;
                }
            }

            await _next(context);
        }
    }

}
