using Bank.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Services.Onboarding.Interfaces
{
    public interface IDocumentService
    {
        ServiceResponse Insert();

        ServiceResponse InsertFromAddress(int customerId);
    }
}
