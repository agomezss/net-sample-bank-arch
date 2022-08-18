using Bank.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Services.Onboarding.Interfaces
{
    public interface IOnboardingService
    {
        ServiceResponse goToNextOnboardingStep(int customerId);
    }
}
