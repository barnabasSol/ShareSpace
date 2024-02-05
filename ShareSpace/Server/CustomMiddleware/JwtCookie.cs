namespace ShareSpace.Server.CustomMiddleware
{
    public class JwtCookie
    {
        private readonly RequestDelegate _next;

        public JwtCookie(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Cookies.TryGetValue("ShareSpaceAccessToken", out var token))
            {
                context.Request.Headers.Append("Authorization", $"Bearer {token}");
            }
            await _next(context);
        }
    }
}
