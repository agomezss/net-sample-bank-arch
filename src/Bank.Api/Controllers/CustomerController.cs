using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Bank.Core.Models;
using Bank.Services.Onboarding.Interfaces;
using Bank.Services.Onboarding.ViewModels;
using System;

namespace Bank.Services.Api.Controllers
{
    public class CustomerController : BankApiController
    {
        private readonly ICustomerService _customerService;
        public CustomerController(IConfiguration config,
                                  ICustomerService customerService)
            : base(config)
        {
            _customerService = customerService;
        }

        [HttpPost("postAddressInfo")]
        [Authorize]
        public ApiResponse PostAddressInfo([FromBody] PostAddressInfoRequest viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                viewModel.Address.CustomerId = BankUser.GetCustomerIdFor(User);

                var addressId = _customerService.SaveCustomerAddressAndUpdateOnboardingStep(viewModel);

                return Response();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
