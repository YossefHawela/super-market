using Microsoft.AspNetCore.Mvc.Filters;
using SuperMarket.DTO;

namespace SuperMarket.Filters
{
    public static class LogFilterDetailedDescription
    {
        public static string DescripeAction(this ActionExecutingContext context)
        {

            var ExcuterName = context.HttpContext.User.Identity!.Name ?? "UnKnown";


            var action = (context.ActionDescriptor.DisplayName?? "Unknows.Unknows Unknows").Split(' ')[0];



            var parameters = string.Join("=>", context.ActionArguments.Select(p=>$"{p.Key}:{p.Value}"));

            string output = $"{ExcuterName} excuted the action '{action}' with the following parameters '{parameters}'";

            return output;
        }

        public static LogDTO ToLogDTO(this ActionExecutingContext context) 
        {
            var ExcuterName = context.HttpContext.User.Identity!.Name ?? "UnKnown";

            return new LogDTO
            {
                UserName = ExcuterName,
                logInforamtion = context.DescripeAction()
            };
        }
    }
}
