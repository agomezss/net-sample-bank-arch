using System;

namespace Bank.Services.Onboarding.ViewModels
{

  public interface IOnboardingActivity {

    string ActivityLogId { get; set; }

    string Channel { get; set; }

    DateTime TransactionTime { get; set; }

    string Origin { get; set; }

    string Category { get; set; }

    string ActivityJsonData { get; set; }
  }
}
