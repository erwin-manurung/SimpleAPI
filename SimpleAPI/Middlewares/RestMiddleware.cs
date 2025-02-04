namespace SimpleAPI.Middlewares
{
    public class RestMiddleware
    {
        private readonly RequestDelegate _next;
        public async Task InvokeAsync(Microsoft.AspNetCore.Http.HttpContext httpContext)
        {
            await _next(httpContext);
        }
    }
}
