using System;
using System.Linq;
using Bank.Core.Context;
using Bank.Core.Models;
using Bank.Core.ViewModels;
using Bank.Services.Onboarding.Interfaces;
using Bank.Services.Onboarding.ViewModels;

namespace Bank.Services.Onboarding
{
    public class PhoneService : IPhoneService
    {
        private readonly BankDbContext _db;
        

        public PhoneService(BankDbContext db)
        {
            _db = db;
        }

        public Phone GetPhoneFromModel(PostProspectInformationRequest request)
        {
            var phone = new Phone
            {
                PhoneUid = request.PhoneUid,
                CountryCode = request.MobileCountryCode.Replace("+", "").Trim(),
                AreaCode = request.MobileLocalAreaCode,
                PhoneNumber = request.MobileNumber
            };

            return phone;
        }

        public PostVerifyPhoneSubscriptionResponse VerifyPhoneSubscription(MobilePhoneActivityRequest requestData)
        {
            var response = new PostVerifyPhoneSubscriptionResponse();

            var phone = _db.Phones.FirstOrDefault(p => p.PhoneUid == requestData.PhoneUid);

            if (phone != null)
            {
                var customer = _db.Customers.Find(phone.CustomerId);

                response.SubscriptionExists = true;
                response.CustomerFirstName = customer.GetFirstName();
            }

            return response;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
