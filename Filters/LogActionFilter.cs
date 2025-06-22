using Microsoft.AspNetCore.Mvc.Filters;
using SuperMarket.Data;
using SuperMarket.DTO;

namespace SuperMarket.Filters
{
    public class LogActionFilter: IActionFilter
    {
        private readonly ILogger<LogActionFilter> _logger;
        private readonly DataConnector _dataConnector;
        public LogActionFilter(ILogger<LogActionFilter> logger,DataConnector dataConnector)
        {
            _logger = logger;
            _dataConnector = dataConnector;
        }
        
        void IActionFilter.OnActionExecuted(ActionExecutedContext context)
        {

        }

        void IActionFilter.OnActionExecuting(ActionExecutingContext context)
        {

            _dataConnector.Add(context.ToLogDTO());

            _logger.LogInformation(context.DescripeAction());
        }
    }
}
