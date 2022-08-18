using Amazon.Lambda.Core;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Bank.Core.ViewModels;
using Bank.Services.Authentication.Interfaces;
using Bank.Services.Onboarding.Interfaces;
using System;

using System.Threading.Tasks;

namespace Bank.Services.Api.Controllers
{
    [Route("api/[controller]")]
    public class AuthenticationController : BankApiController
    {
        private IAuthenticationService _authService;
        private IPhoneService _phoneService;

        public AuthenticationController(IConfiguration config, IAuthenticationService authService, IPhoneService phoneService) : base(config)
        {
            _authService = authService;

            _phoneService = phoneService;
        }

        [HttpPost("loginWithPasscode")]
        public async Task<ApiResponse> LoginWithPasscode([FromBody] LoginWithPasscodeRequest request)
        {
            if (User.Identity.IsAuthenticated)
            {
                //user already authenticated; problem?
            }

            if (ModelState.IsValid)
            {
                var serviceResponse = await _authService.LoginWithPasscode(request);

                return Response(serviceResponse);
            }
            else
            {
                return await BadRequestAsync();
            };
        }

        [HttpPost]
        [Route("verifyPhoneSubscription")]
        public ApiResponse VerifyPhoneSubscription([FromBody] MobilePhoneActivityRequest request)
        {
            try
            {
                return Response(_phoneService.VerifyPhoneSubscription(request));
            }
            catch (Exception ex)
            {
                //_outputLogger.Log($"{nameof(AuthenticationController)} {nameof(PostVerifyPhoneSubscription)} error: {ex.Message}");
                
                return InternalServerError(ex);
            }
        }
    }
}
