using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Bank.Api.Logging;
using Bank.Core.Interfaces;

namespace Bank.Api
{
    public class LogRequestResponseAttribute : ActionFilterAttribute
    {
        private INoSqlClient _logger;

        public LogRequestResponseAttribute() { }

        private void LoadDependencies(FilterContext context)
        {
            _logger = _logger ?? context.HttpContext.RequestServices.GetService(typeof(INoSqlClient)) as INoSqlClient;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            try
            {
                LoadDependencies(context);

                var descriptor = context.ActionDescriptor as ControllerActionDescriptor;

                var actionName = descriptor.ActionName;
                var controllerName = descriptor.ControllerName;
                var requestHeadersText = CommonLoggingTools.SerializeHeaders(context.HttpContext.Request.Headers);
                var requestResponseData = CommonLoggingTools.FormatRequestBody(context.HttpContext.Request)?.Result;
                var responseHeadersText = CommonLoggingTools.SerializeHeaders(context.HttpContext.Response.Headers);

                var statusCode = context.HttpContext.Response.StatusCode;
                var result = context.Result as ObjectResult;

                requestResponseData.Action = actionName;
                requestResponseData.Controller = controllerName;
                requestResponseData.RequestHeaders = requestHeadersText;
                requestResponseData.ResponseHeaders = responseHeadersText;
                requestResponseData.StatusCode = statusCode;
                requestResponseData.ResponseBody = JsonConvert.SerializeObject(result.Value);
                //requestResponseData.User = context.HttpContext.User // TODO: Get Logged User Info

                _logger.InsertNoWait("RequestLog", requestResponseData);
            }
            catch
            {
            }
        }
    }
}
