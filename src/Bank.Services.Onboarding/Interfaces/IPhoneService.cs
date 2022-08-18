using System;
using Bank.Core.Models;
using Bank.Core.ViewModels;
using Bank.Services.Onboarding.ViewModels;

namespace Bank.Services.Onboarding.Interfaces
{
    public interface IPhoneService : IDisposable
    {
        Phone GetPhoneFromModel(PostProspectInformationRequest fromRequest);

        PostVerifyPhoneSubscriptionResponse VerifyPhoneSubscription(MobilePhoneActivityRequest requestData);
    }
}
