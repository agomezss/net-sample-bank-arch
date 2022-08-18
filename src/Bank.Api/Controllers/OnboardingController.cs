using Bank.Core.Models;
using Bank.Core.ViewModels;
using Bank.Services.Onboarding.Interfaces;
using Bank.Services.Onboarding.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace Bank.Services.Api.Controllers
{
    [Route("api/[controller]")]
    public class OnboardingController : BankApiController
    {
        private readonly IAddressService _addressService;
        private readonly IOnboardingService _obService;

        public OnboardingController(IConfiguration config,
                                    IAddressService addressService,
                                    IOnboardingService obService) : base(config)
        {
            _addressService = addressService;
            _obService = obService;
        }

        [HttpPost("postAddressAndUpdateOnboarding")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ApiResponse postAddressAndUpdateOnboarding([FromBody] PostAddressInfoRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                request.Address.CustomerId = BankUser.GetCustomerIdFor(HttpContext.User);

                return Response(_addressService.InsertAddressAndUpdateOnboarding(request));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost("postOnboardingDocumentSent")]
        public ApiResponse postOnboardingDocumentSent([FromBody] BaseRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                return Response(_obService.goToNextOnboardingStep(BankUser.GetCustomerIdFor(User)));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        //        [HttpGet]
        //        public ApiResponse GetData()
        //        {
        //            return new ApiResponse("getData Test OK");
        //        }
        //
        //        [HttpPost("postStepPassed")]
        //        public ApiResponse PostStepPassed([FromBody] PostOnboardingStepPassedRequest requestData)
        //        {
        //            _outputLogger.Log($"Received onboarding stepPassed request: {JsonConvert.SerializeObject(requestData)}");
        //
        //            ApiResponse response = new ApiResponse("ProspectActivityLog_Insert Request Sent.");
        //
        //            if (requestData != null)
        //            {
        //                _noSqlLogger.Log("ProspectActivityLog", requestData);
        //            }
        //            else
        //            {
        //                response.SetBadRequest();
        //            }
        //            
        //            return response;
        //        }
        //
        //        [HttpPost("postProspectInformation")]
        //        public async Task<ApiResponse> PostProspectInformation([FromBody] PostProspectInformationRequest requestData)
        //        {
        //            _outputLogger.Log($"Received postProspectInformation request: {JsonConvert.SerializeObject(requestData)}");
        //
        //            var response = new ApiResponse();
        //
        //            if (ModelState.IsValid)
        //            {
        //                /*
        //                var vc = new ValidationContext(requestData);
        //                var vResults = new List<ValidationResult>();
        //                var isValid = Validator.TryValidateObject(requestData, vc, vResults);
        //                */
        //
        //                _activityLogger.StartExtendedActivity("onboardingController.postProspectInformation", "insertCustomer");
        //
        //                try
        //                {
        //                    //Step 1: Save prospect information as Customer on the database
        //                    Customer newCustomer = _customerService.InsertCustomer(requestData);
        //
        //                    _activityLogger.LogAndSwitchActivityFormat(
        //                      "createCustomerUser",
        //                      "Customer [{0}] mobile [{1}] created with customerId [{2}]",
        //                      newCustomer.Name,
        //                      newCustomer.Phones.First().ToString(),
        //                      newCustomer.CustomerId);
        //
        //                    //Step 2: Create an aspNetUser for the customer, let's use phoneUid as UserName and the passcode as password for now...
        //                    BankUser newCustomersUser = new BankUser() { Email = newCustomer.Email, UserName = requestData.PhoneUid, Name = newCustomer.Name };
        //
        //                    await _userManager.CreateAsync(newCustomersUser, requestData.Passcode);
        //
        //                    _activityLogger.LogAndSwitchActivityFormat(
        //                      "updateCustomerWithUser",
        //                      "Customer user for customerId [{0}] created with aspNetUserId [{1}]",
        //                      newCustomer.CustomerId,
        //                      newCustomersUser.Id);
        //
        //                    //Setp 3: Link the new user with the customer. Ideally this should be in a transaction but the info is recoverable through the email property
        //                    _customerService.UpdateCustomerUser(newCustomer, newCustomersUser);
        //
        //                    _activityLogger.LogAndSwitchActivity(
        //                      "sendMobileConfirmationMessage",
        //                      "Customer updated with user information.");
        //
        //                    //Step 4: Login automatically after creating the customer and customerUSer 
        //                    //TODO: this has to be login information
        //
        //                    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        //                    JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();
        //
        //                    var claims = new[] {
        //                        new Claim(JwtRegisteredClaimNames.Sub, newCustomer.CustomerId.ToString()),
        //                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //                        new Claim(JwtRegisteredClaimNames.UniqueName, newCustomersUser.UserName)
        //                      };
        //
        //                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
        //                    var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //                    var token = new JwtSecurityToken(
        //                      _config["Tokens:Issuer"],
        //                      _config["Tokens:Audience"],
        //                      claims,
        //                      null,
        //                      DateTime.UtcNow.AddMinutes(30),
        //                      credential
        //                    );
        //
        //                    var result = new
        //                    {
        //                        token = new JwtSecurityTokenHandler().WriteToken(token),
        //                        expiration = token.ValidTo,
        //                        customer = newCustomer
        //                    };
        //
        //                    response.SetResponseObject(result);
        //
        //                    //Step 5: Create phone confirmation via api
        //                    try
        //                    {
        //                        //_phoneConfirmationCodeService.SendMobileConfirmationMessage(newCustomer.Phones.First());
        //                    }
        //                    catch (Exception exTwilio)
        //                    {
        //                        //TODO: not really an exception to crash the whole process... gotta figure a way to retry from the UI. 
        //                        //Do something here to notify the UI to auto-resend the code
        //                        _activityLogger.LogError(exTwilio, false);
        //                    }
        //
        //                    //Step 6: Send operation log to dynamo
        //                    try
        //                    {
        //                        _activityLogger.SwitchActivityAndLog("prospectActivityLog_Insert");
        //                        _noSqlLogger.Log("prospect", requestData);
        //
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        _activityLogger.LogError(ex, false);
        //                    }
        //
        //                }
        //                catch (Exception exInsertCustomer)
        //                {
        //                    _activityLogger.LogError(exInsertCustomer, false);
        //                    response.SetInternalServerError(exInsertCustomer);
        //                }
        //
        //                _activityLogger.FinishExtendedActivity();
        //
        //                // Rafactor: Check necessity
        //                //response.FinishResponseLog(logId);
        //
        //            }
        //            else
        //            {
        //                response.SetBadRequest(ModelState);
        //            }
        //
        //            return response;
        //
        //        }

    }
}
