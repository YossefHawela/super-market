
namespace SuperMarket.Middleware
{
    public class PreventSqlInjectionMiddleware : IMiddleware
    {
        private static readonly string[] SqlInjectionKeywords =
        {
            "select", "insert", "update", "delete", "drop", "--", ";--", ";", "/*", "*/", "@@",
            "char", "nchar", "varchar", "nvarchar", "alter", "begin", "cast", "create",
            "cursor", "declare", "exec", "fetch", "kill", "open", "sys", "sysobjects", "syscolumns"
        };

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var query = context.Request.QueryString.ToString().ToLower();
            var path = context.Request.Path.ToString().ToLower();

            foreach (var keyword in SqlInjectionKeywords)
            {
                if (query.Contains(keyword))
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;

                    context.Response.Redirect($"/Home/Blocked?keyword='{string.Join('_',keyword.ToCharArray())}'");

                    return;
                }
            }

            await next(context);

        }
    }
}
