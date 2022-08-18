using Microsoft.AspNetCore.Identity;
using Bank.Core;
using Bank.Core.Models;
using Bank.Services.Onboarding.ViewModels;
using System;
using System.Threading.Tasks;

namespace Bank.Services.Onboarding.Interfaces
{
    public interface IProspectService : IDisposable
    {
        Task<ServiceResponse> Register(ProspectViewModel model);
        
        ProspectViewModel GetById(int prospectId, string phoneNumber);

        ServiceResponse Update(ProspectViewModel model);

        ServiceResponse ProceedToNextOnboardingStep(int customerId);
    }
}
