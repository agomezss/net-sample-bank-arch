using Bank.Core;
using Bank.Services.Onboarding.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Services.Onboarding.Interfaces
{
    public interface IAddressService
    {
        ServiceResponse InsertAddressAndUpdateOnboarding(PostAddressInfoRequest request);
    }
}
