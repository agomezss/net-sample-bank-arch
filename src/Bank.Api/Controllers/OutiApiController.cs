using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Bank.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Services.Api.Controllers
{
    public abstract class BankApiController : ControllerBase
    {
        protected readonly IConfiguration _config;

        protected BankApiController(IConfiguration config)
        {
            _config = config;
        }

        protected bool IsValidOperation()
        {
            return true;
            //return (!_notifications.HasNotifications());
        }

        protected ApiResponse InternalServerError(Exception ex)
        {
            var response = new ApiResponse();
            response.SetInternalServerError(ex);
            return response;
        }

        protected ApiResponse Unauthorized(string message = "You have not permission to perform this action.")
        {
            var response = new ApiResponse();
            response.SetUnautorized(message);
            return response;
        }
        
        protected Task<ApiResponse> BadRequestAsync()
        {
            var response = new ApiResponse();

            response.SetBadRequest();
            
            return Task.FromResult<ApiResponse>(response);
        }
        
        protected new ApiResponse BadRequest()
        {
            var response = new ApiResponse();

            response.SetBadRequest();
            
            return response;
        }

        protected new ApiResponse BadRequest(ModelStateDictionary modelErrors)
        {
            var response = new ApiResponse();
            response.SetBadRequest(modelErrors);
            return response;
        }

        protected new ApiResponse Response(ServiceResponse serviceResponse)
        {
            var response = new ApiResponse(serviceResponse);

            return response;
        }

        protected new ApiResponse Response(object result = null)
        {
            var response = new ApiResponse(result);

            if (IsValidOperation())
            {
                return response;
            }

            response.SetBadRequest();

            return response;
        }

        protected ApiResponse Success()
        {
            return Response();
        }

        protected void NotifyModelStateErrors()
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                var errorMsg = error.Exception == null ? error.ErrorMessage : error.Exception.Message;
                NotifyError(string.Empty, errorMsg);
            }
        }

        protected void NotifyError(string code, string message)
        {
            //_mediator.RaiseEvent(new DomainNotification(code, message));
        }

        protected void AddIdentityErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                NotifyError(result.ToString(), error.Description);
            }
        }
    }
}
