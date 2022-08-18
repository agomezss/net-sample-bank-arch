using Bank.Core;
using Bank.Core.Context;
using Bank.Core.Models;
using Bank.Services.Onboarding.Interfaces;
using Bank.Services.Onboarding.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Services.Onboarding
{
    public class AddressService : IAddressService
    {
        private readonly BankDbContext _db;

        public AddressService(BankDbContext db)
        {
            _db = db;
        }

        public ServiceResponse InsertAddressAndUpdateOnboarding(PostAddressInfoRequest request)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var newAddress = request.Address;

                _db.BeginTransaction();

                _db.Addresses.Add(newAddress);

                var newAddressDocument = new Document()
                {
                    CustomerId = newAddress.CustomerId,
                    DocumentNumber = string.Format("{0} {1} {2}", newAddress.AddressName, newAddress.AddressNumber, newAddress.Complement),
                    DocumentStatusId = BankTypes.CONST_DOC_STATUS_RECEIVED.DocumentStatusId,
                    DocumentTypeId = BankTypes.CONST_DOC_TYPE_PROOF_OF_ADDRESS.DocumentTypeId
                };

                _db.Documents.Add(newAddressDocument);

                _db.SaveChanges();

                var newDocDetails = new DocumentDetail()
                {
                    DocumentId = newAddressDocument.DocumentId,
                    DocumentStatusId = BankTypes.CONST_DOC_STATUS_RECEIVED.DocumentStatusId,
                    Detail = "Document received"
                };

                _db.DocumentDetails.Add(newDocDetails);

                var customer = _db.Customers.Find(newAddress.CustomerId);

                customer.OnboardingStepId = BankTypes.CONST_STEP_PROOF_OF_ADDRESS_UPLOAD_REQUESTED.OnboardingStepId;

                _db.OnboardingStepHistories.Add(new OnboardingStepHistory()
                {
                  CustomerId = customer.CustomerId,
                  OnboardingStepId = BankTypes.CONST_STEP_ADDRESS_PROVIDED.OnboardingStepId
                });
                
                _db.Commit();

                _db.SaveChanges();
                
                response.Data = new
                {
                    addressId = newAddress.AddressId,
                    documentId = newAddressDocument.DocumentId,
                    onboardingStep = BankTypes.CONST_STEP_PROOF_OF_ADDRESS_UPLOAD_REQUESTED
                };
            }
            catch (Exception ex)
            {
                try
                {
                    _db.Rollback();
                }
                catch { }

                response.AddError(ex);
            }

            return response;
        }
    }
}
