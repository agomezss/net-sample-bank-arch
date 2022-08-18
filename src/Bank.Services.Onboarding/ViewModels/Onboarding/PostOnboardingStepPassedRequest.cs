namespace Bank.Services.Onboarding.ViewModels
{
    public class PostOnboardingStepPassedRequest : OnboardingBaseRequest
    {
        public string StepKey { get; set; }
        public PostOnboardingStepPassedRequest() : base() { }
    }
}
