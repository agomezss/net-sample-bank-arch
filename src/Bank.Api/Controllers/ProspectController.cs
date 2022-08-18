using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Bank.Core.Interfaces;
using Bank.Core.Models;
using Bank.Services.Onboarding.Interfaces;
using Bank.Services.Onboarding.ViewModels;
using System;
using System.Threading.Tasks;

namespace Bank.Services.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProspectController : BankApiController
    {
        private readonly IProspectService _prospectService;
        
        private readonly INoSqlLogger _noSqlLogger;

        private readonly IOutputLogger _outputLogger;

        public ProspectController(IConfiguration config,
                                  IProspectService prospectService,
                                  INoSqlLogger noSqlLogger,
                                  IOutputLogger outputLogger) : base(config)
        {
            _prospectService = prospectService;

            _noSqlLogger = noSqlLogger;

            _outputLogger = outputLogger;
        }
        
        [HttpPost("postStepPassed")]
        public ApiResponse PostStepPassed([FromBody] PostOnboardingStepPassedRequest request)
        {
            _outputLogger.Log($"Received onboarding stepPassed request: {JsonConvert.SerializeObject(request)}");

            ApiResponse response = new ApiResponse("ProspectActivityLog_Insert Request Sent.");

            if (request != null)
            {
                _noSqlLogger.Log("ProspectActivityLog", request);
            }
            else
            {
                response.SetBadRequest();
            }
            
            return response;
        }

        [HttpPost]
        [AllowAnonymous]
        //[Route("postProspectInformation")]
        public ApiResponse Post([FromBody]ProspectViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _prospectService.Register(viewModel);

                return Response(viewModel);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        //[Authorize(Policy = "CanUpdateProspectData")]
        //[Route("postProspectInformation")]
        public async Task<ApiResponse> Put([FromBody]ProspectViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _prospectService.Register(viewModel);

                return Response(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
