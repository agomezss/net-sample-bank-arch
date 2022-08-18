using Newtonsoft.Json;
using Bank.Core;
using Bank.Core.Context;
using Bank.Core.Models;
using Bank.Services.Onboarding.Interfaces;
using Bank.Services.Onboarding.ViewModels;
using System;
using System.Linq;

namespace Bank.Services.Onboarding
{
    public class CustomerService : ICustomerService
    {
        private readonly BankDbContext _db;

        public CustomerService(BankDbContext db)
        {
            _db = db;
        }

        public ServiceResponse SaveCustomerAddressAndUpdateOnboardingStep(PostAddressInfoRequest address)
        {
            var response = new ServiceResponse();

            try
            {
                _db.BeginTransaction();

                var addressResponse = SaveCustomerAddress(address);

                SetCustomerOnboardingStep(address.Address.CustomerId, BankTypes.CONST_STEP_ADDRESS_PROVIDED.OnboardingStepId);

                _db.Commit();

                response.Data = new { addressData = addressResponse, onboardingStep = BankTypes.CONST_STEP_ADDRESS_PROVIDED };
            }
            catch (Exception ex)
            {
                response.AddError(ex);
                _db.Rollback();
            }

            return response;
        }

        public void UpdateCustomerOnboardingStep(int customerId, int onboardingStepId)
        {
            UpdateCustomerOnboardingStep(customerId, onboardingStepId);
            _db.Commit();
        }

        private void SetCustomerOnboardingStep(int customerId, int onboardingStepId)
        {
            var customer = _db.Customers.Find(customerId);

            customer.OnboardingStepId = onboardingStepId;

            _db.OnboardingStepHistories.Add(new OnboardingStepHistory() 
            { CustomerId = customerId, OnboardingStepId = onboardingStepId });
        }

        private object SaveCustomerAddress(PostAddressInfoRequest address)
        {
            var newAddress = address.Address;

            newAddress.CountryId = BankTypes.Countries
              .Find(c => c.Abbreviation == newAddress.Country.Abbreviation).CountryId;

            _db.Addresses.Add(newAddress);

            var newAddressDocument = new Document()
            {
                CustomerId = newAddress.CustomerId,
                DocumentNumber = string.Format("{0} {1} {2}", newAddress.AddressName, newAddress.AddressNumber, newAddress.Complement),
                DocumentDetails = JsonConvert.SerializeObject(newAddress),
                DocumentTypeId = BankTypes.CONST_DOC_TYPE_PROOF_OF_ADDRESS.DocumentTypeId,
                DocumentStatusId = BankTypes.CONST_DOC_STATUS_RECEIVED.DocumentStatusId
            };

            _db.Documents.Add(newAddressDocument);

            var newDocumentDetail = new DocumentDetail()
            {
                DocumentId = newAddressDocument.DocumentId,
                DocumentStatusId = BankTypes.CONST_DOC_STATUS_RECEIVED.DocumentStatusId
            };

            _db.DocumentDetails.Add(newDocumentDetail);

            return new { addressId = newAddress.AddressId, addressDocumentId = newAddressDocument.DocumentId, documentDetailId = newDocumentDetail.DocumentDetailId };
        }

        public Customer InsertCustomer(PostProspectInformationRequest fromRequest)
        {
            throw new NotImplementedException();
        }

        public void UpdateCustomerUser(Customer customer, BankUser customerUser)
        {
            throw new NotImplementedException();
        }
    }
}
