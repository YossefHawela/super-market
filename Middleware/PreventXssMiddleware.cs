namespace SuperMarket.Middleware
{
    public class PreventXssMiddleware : IMiddleware
    {
        private static readonly string[] XssIndicators =
        {
            "<script", "</script", "javascript:", "onerror", "onload", "eval(", "alert(", "document.", "window."
        };

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var query = context.Request.QueryString.ToString().ToLower();
            var path = context.Request.Path.ToString().ToLower();

            foreach (var suspect in XssIndicators)
            {
                if (query.Contains(suspect) || path.Contains(suspect))
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;

                    context.Response.Redirect($"/Home/Blocked?keyword='{string.Join('_', suspect.ToCharArray())}'");

                    return;
                }
            }

            await next(context);
        }
    }

}
