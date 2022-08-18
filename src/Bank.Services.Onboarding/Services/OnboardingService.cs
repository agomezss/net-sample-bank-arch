using Bank.Core;
using Bank.Core.Context;
using Bank.Core.Models;
using Bank.Services.Onboarding.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Services.Onboarding
{
    public class OnboardingService : IOnboardingService
    {
        private readonly BankDbContext _db;

        public OnboardingService(BankDbContext db)
        {
            _db = db;
        }

        public ServiceResponse goToNextOnboardingStep(int customerId)
        {
            ServiceResponse response = new ServiceResponse();

            var customer = _db.Customers.Find(customerId);

            int stepId = customer.OnboardingStepId;

            OnboardingStep theStep = new OnboardingStep();

            while (stepId != -1)
            {
                theStep = BankTypes.OnboardingSteps.Find(s => s.OnboardingStepId == stepId);

                stepId = -1;

                if (theStep.NextStepId.HasValue)
                {
                    theStep = BankTypes.OnboardingSteps.Find(s => s.OnboardingStepId == theStep.NextStepId.Value);

                    if (theStep != null)
                    {
                        //sanity check

                        customer.OnboardingStepId = theStep.OnboardingStepId;

                        _db.OnboardingStepHistories.Add(new OnboardingStepHistory()
                        {
                            CustomerId = customer.CustomerId,
                            OnboardingStepId = theStep.OnboardingStepId
                        });

                        _db.SaveChanges();

                        if (theStep.Description.ToLower().Contains("provided"))
                        {
                            // across most steps, it goes from 'provided' to 'requested'; but for CUSTOMER SELFIE PROVIDED we have
                            // then after UPLOADS PROVIDED so we need this to be a while loop to go to the next: TERMS & CONDITIONS REQUEST
                            // so the rule is whenever there's a 'provided' word in the description, it's gonna go to the next one that doesn't
                            // have it, no matter how many steps ahead.

                            stepId = theStep.OnboardingStepId;
                        }
                    }
                }
            }

            response.Data = new
            {
                result = "ok",
                onboardingStep = theStep
            };

            return response;
        }
    }
}
