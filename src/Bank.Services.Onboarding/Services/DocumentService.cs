using Newtonsoft.Json;
using Bank.Core;
using Bank.Core.Context;
using Bank.Core.Models;
using Bank.Services.Onboarding.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bank.Services.Onboarding
{
    public class DocumentService : IDocumentService
    {
        private readonly BankDbContext _db;

        public DocumentService(BankDbContext db)
        {
            _db = db;
        }

        public ServiceResponse Insert()
        {
            throw new NotImplementedException();
        }

        public ServiceResponse InsertFromAddress(int customerId)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                var address = _db.Addresses.FirstOrDefault(a => a.CustomerId == customerId);

                if (address != null)
                {
                    Document newDoc = new Document()
                    {
                        CustomerId = customerId,
                        DocumentStatusId = BankTypes.CONST_DOC_STATUS_RECEIVED.DocumentStatusId,
                        DocumentTypeId = BankTypes.CONST_DOC_TYPE_PROOF_OF_ADDRESS.DocumentTypeId,
                        DocumentNumber = string.Format("{0} {1} {2}", address.AddressName, address.AddressNumber, address.Complement),
                        DocumentDetails = JsonConvert.SerializeObject(address)
                    };

                    _db.Documents.Add(newDoc);

                    _db.SaveChanges();

                    response.Data = newDoc;
                }
            }
            catch (Exception ex)
            {
                response.AddError(ex);
            }

            return response;
        }
    }
}
