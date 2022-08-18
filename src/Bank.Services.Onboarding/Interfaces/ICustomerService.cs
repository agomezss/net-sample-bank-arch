using Bank.Core;
using Bank.Core.Models;
using Bank.Core.ViewModels;
using Bank.Services.Onboarding.ViewModels;
using System.Security.Principal;

namespace Bank.Services.Onboarding.Interfaces
{
    public interface ICustomerService
    {
        ServiceResponse SaveCustomerAddressAndUpdateOnboardingStep(PostAddressInfoRequest address);

        Customer InsertCustomer(PostProspectInformationRequest fromRequest);
        
        void UpdateCustomerUser(Customer customer, BankUser customerUser);
        
        void UpdateCustomerOnboardingStep(int customerId, int onboardingStepId);
    }
}
