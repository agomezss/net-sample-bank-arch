using System;
using Bank.Core;
using Bank.Core.ViewModels;
using Bank.Services.Onboarding.ViewModels.Onboarding;

namespace Bank.Services.Onboarding.Interfaces
{
    public interface IPhoneConfirmationCodeService : IDisposable
    {
        ServiceResponse VerifyPhoneCode(PostPhoneVerificationCodeRequest request);

        ServiceResponse GenerateConfirmationCode(PostProspectGenerateConfirmationCode request);
    }
}
