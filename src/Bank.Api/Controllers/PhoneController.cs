using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Bank.Api;
using Bank.Core.ViewModels;
using Bank.Services.Onboarding.Interfaces;
using Bank.Services.Onboarding.ViewModels.Onboarding;
using System;

namespace Bank.Services.Api.Controllers
{
    [Route("api/[controller]")]
    public class PhoneController : BankApiController
    {
        private readonly IPhoneService _phoneService;
        private readonly IPhoneConfirmationCodeService _phoneConfirmationCodeService;

        public PhoneController(IConfiguration config,
                               IPhoneService phoneService,
                               IPhoneConfirmationCodeService phoneConfirmationCodeService)
            : base(config)
        {
            _phoneService = phoneService;
            _phoneConfirmationCodeService = phoneConfirmationCodeService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(ValidateConfirmationCode))]
        public ApiResponse ValidateConfirmationCode([FromBody]PostPhoneVerificationCodeRequest viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                return Response(_phoneConfirmationCodeService.VerifyPhoneCode(viewModel));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [LogRequestResponse]
        [Route(nameof(GenerateConfirmationCode))]
        public ApiResponse GenerateConfirmationCode([FromBody]PostProspectGenerateConfirmationCode viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                return Response(_phoneConfirmationCodeService.GenerateConfirmationCode(viewModel));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
