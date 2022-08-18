using Bank.Core;
using Bank.Core.Models;
using Bank.Core.ViewModels;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Bank.Services.Authentication.Interfaces
{
    public interface IAuthenticationService
    {
        Task<ServiceResponse> LoginWithPasscode(LoginWithPasscodeRequest request);
    }
}
