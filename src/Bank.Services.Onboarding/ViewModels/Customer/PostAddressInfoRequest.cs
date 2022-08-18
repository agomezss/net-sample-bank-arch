using Bank.Core.Models;
using Bank.Core.ViewModels;

namespace Bank.Services.Onboarding.ViewModels
{
    public class PostAddressInfoRequest : BaseRequest
    {
        public Address Address { get; set; }
    }
}
