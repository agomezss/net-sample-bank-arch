using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Bank.Api.Logging;
using Bank.Core.Interfaces;

namespace Bank.Api
{
    public class LogRequestAttribute : ActionFilterAttribute
    {
        private INoSqlClient _logger;

        public LogRequestAttribute() { }

        private void LoadDependencies(FilterContext context)
        {
            _logger = _logger ?? context.HttpContext.RequestServices.GetService(typeof(INoSqlClient)) as INoSqlClient;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                LoadDependencies(context);

                var descriptor = context.ActionDescriptor as ControllerActionDescriptor;

                var actionName = descriptor.ActionName;
                var controllerName = descriptor.ControllerName;

                var result = context.Result;
                var requestHeadersText = CommonLoggingTools.SerializeHeaders(context.HttpContext.Request.Headers);
                var requestResponseData = CommonLoggingTools.FormatRequestBody(context.HttpContext.Request)?.Result;

                requestResponseData.Action = actionName;
                requestResponseData.Controller = controllerName;
                requestResponseData.RequestHeaders = requestHeadersText;
                //requestResponseData.User = context.HttpContext.User // TODO: Get Logged User Info

                _logger.InsertNoWait("RequestLog", requestResponseData);
            }
            catch
            {
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            
        }
    }
}
