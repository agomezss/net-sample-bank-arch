using Bank.Core.ViewModels;

namespace Bank.Services.Onboarding.ViewModels
{
    public class OnboardingBaseRequest : ActivityLogBaseRequest
    {
        public OnboardingBaseRequest() : base()
        {
            Category = "onboarding";
        }
    }
}
