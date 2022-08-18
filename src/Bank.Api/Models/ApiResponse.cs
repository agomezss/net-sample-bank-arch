using Amazon.Lambda.APIGatewayEvents;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Bank.Core;
using System;
using System.Collections.Generic;
using System.Net;

namespace Bank.Services.Api
{
    public class ApiResponse : APIGatewayProxyResponse
    {
        [JsonProperty]
        public object BodyObject { get; set; }

        [JsonProperty]
        public string ResponseLog { get; set; }

        [JsonProperty]
        public new string Body
        {
            get
            {
                return JsonConvert.SerializeObject(BodyObject);
            }

            set { }
        }

        private readonly bool _isProduction;
        private readonly bool _useLambdaResultObjectResponse;

        public ApiResponse(object responseObject = null) : base()
        {
            _isProduction = BankApiSettings.IsProduction;
            _useLambdaResultObjectResponse = BankApiSettings.UseLambdaResultObjectResponse;

            StatusCode = 200;

            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } };

            if (responseObject != null)
            {
                if (responseObject.GetType() == typeof(ServiceResponse))
                {
                    if (((ServiceResponse)responseObject).HasErrors())
                    {
                        StatusCode = 500;
                        
                        BodyObject = ((ServiceResponse)responseObject).GetErrors();
                    }
                    else
                    {
                        BodyObject = ((ServiceResponse)responseObject).Data;
                    }
                }
                else
                {
                    BodyObject = responseObject;
                }
            }
        }

        public bool ShouldSerializeBodyObject()
        {
            return !_isProduction && !_useLambdaResultObjectResponse;
        }

        public bool ShouldSerializeResponseLog()
        {
            return !_isProduction && !_useLambdaResultObjectResponse;
        }

        public bool ShouldSerializeBody()
        {
            return _isProduction || _useLambdaResultObjectResponse;
        }

        public void SetBadRequest(ServiceResponse response)
        {
            StatusCode = (int)HttpStatusCode.BadRequest;
            BodyObject = response.GetErrors();
        }

        internal void SetUnautorized(string message = "You have not permission to perform this action.")
        {
            StatusCode = (int)HttpStatusCode.Unauthorized;
            BodyObject = message;
        }

        public void SetBadRequest(string message = "Bad Request Params.")
        {
            StatusCode = (int)HttpStatusCode.BadRequest;
            BodyObject = message;
        }

        public void SetBadRequest(ModelStateDictionary modelErrors)
        {
            StatusCode = (int)HttpStatusCode.BadRequest;

            List<object> errors = new List<object>();

            foreach (var errorKey in modelErrors.Keys)
            {
                foreach (var error in modelErrors[errorKey].Errors)
                {
                    errors.Add(new
                    {
                        fieldName = errorKey,
                        errorMessage = error.ErrorMessage
                    });
                }
            }

            BodyObject = errors;
        }

        public void SetInternalServerError(Exception ex)
        {
            StatusCode = (int)HttpStatusCode.InternalServerError;

            BodyObject = new
            {
                errorMessage = ex.Message,
                stackTrace = ex.StackTrace,
                innerError = (ex.InnerException != null) ? ex.InnerException.Message : ""
            };
        }

        public void SetResponseObject(object responseObject, int statusCode = 200)
        {
            StatusCode = statusCode;
            BodyObject = responseObject;
        }
    }
}
