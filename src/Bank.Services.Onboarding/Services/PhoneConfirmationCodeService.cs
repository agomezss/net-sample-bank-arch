using Microsoft.Extensions.Configuration;
using Bank.Core;
using Bank.Core.Context;
using Bank.Core.Interfaces;
using Bank.Core.Models;
using Bank.Core.ViewModels;
using Bank.Services.Onboarding.Interfaces;
using Bank.Services.Onboarding.ViewModels.Onboarding;
using System;
using System.Linq;

namespace Bank.Services.Onboarding
{
    public class PhoneConfirmationService : IPhoneConfirmationCodeService
    {
        private readonly IConfiguration _configuration;
        private readonly BankDbContext _db;
        private readonly INoSqlClient _noSqlClient;
        private readonly ISmsSender _smsSender;
        private readonly ICustomerService _customerService;

        public PhoneConfirmationService(BankDbContext db,
                                        INoSqlClient noSqlClient,
                                        IConfiguration configuration,
                                        ISmsSender smsSender,
                                        ICustomerService customerService)
        {
            _db = db;
            _noSqlClient = noSqlClient;
            _configuration = configuration;
            _smsSender = smsSender;
            _customerService = customerService;
        }

        public ServiceResponse VerifyPhoneCode(PostPhoneVerificationCodeRequest request)
        {
            var response = new ServiceResponse();

            try
            {
                Phone phone = GetPhone(request.PhoneUid);

                if (phone == null)
                {
                    return response.AddError("We can't validate your code.");
                }

                var unusedCode = _db.PhoneConfirmationCodes
                                 .FirstOrDefault(p => p.PhoneId == phone.PhoneId
                                                   && p.ValidUntil >= DateTime.Now
                                                   && p.ConfirmedAt == null
                                                   && p.CancelledAt == null
                                                   && p.ConfirmationCode == request.VerificationCode);

                if (unusedCode == null)
                {
                    return response.AddError("We can't validate your code.");
                }

                unusedCode.Use();

                var nextStep = _db.OnboardingSteps.FirstOrDefault(s => s.Description == "MOBILE PHONE CONFIRMED");

                //_customerService.UpdateCustomerOnboardingStep(phone.CustomerId, nextStep.OnboardingStepId);
                _db.Commit();

                CleanUsedValidationCodesForPhone(request);

            }
            catch (Exception ex)
            {
                return response.AddError(ex);
            }

            return response;
        }

        private Phone GetPhone(string phoneUid)
        {
            var phone = _db.Phones.Where(a => a.PhoneUid == phoneUid)
                                  .FirstOrDefault();

            return phone;
        }

        private void CleanUsedValidationCodesForPhone(PostPhoneVerificationCodeRequest request)
        {
            var phone = GetPhone(request.PhoneUid);

            var invalidCodes = _db.PhoneConfirmationCodes.Where(c => c.PhoneId == phone.PhoneId
                                                             && c.ConfirmedAt == null
                                                             && c.CancelledAt == null
                                                             && c.ValidUntil < DateTime.Now)
                                                         .ToList();

            invalidCodes.ForEach(invalidCode => invalidCode.Cancel("Another code validated"));

            _db.Commit();
        }

        public ServiceResponse GenerateConfirmationCode(PostProspectGenerateConfirmationCode request)
        {
            var response = new ServiceResponse();

            try
            {
                var phone = GetPhone(request.PhoneUid);

                if (phone == null)
                {
                    return response.AddError("We can't validate your code.");
                }

                // TODO: Check if prospect is in correct status for send SMS and avoid DDoS

                var codeLifetimeInMinutes = int.Parse(_configuration["BankSettings:SMSCodeLifeSpanMinutes"]);
                var newCode = PhoneConfirmationCode.Create(phone.PhoneId, codeLifetimeInMinutes);
                _db.PhoneConfirmationCodes.Add(newCode);
                _db.Commit();

                _smsSender.SendMessage($"Here is your Banknvest verification code: {newCode.ConfirmationCode}", phone.ToString());
            }
            catch (Exception ex)
            {
                return response.AddError(ex);
            }

            return response;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
