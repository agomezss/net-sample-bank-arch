using Microsoft.AspNetCore.Identity;
using Bank.Core;
using Bank.Core.Context;
using Bank.Core.Interfaces;
using Bank.Core.Models;
using Bank.Core.ViewModels;
using Bank.Infra.Data.UoW;

using Bank.Services.Authentication.Interfaces;
using System;
using System.Linq;

using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Bank.Services.Authentication.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly AuthDbContext _authDb;
        
        private readonly BankDbContext _db;

        private readonly SignInManager<BankUser> _signInManager;
        private readonly UserManager<BankUser> _userManager;
        private readonly IOutputLogger _outputLogger;
        private readonly IConfiguration _config;
        
        public AuthenticationService(AuthDbContext authDb,
                                     BankDbContext db,
                                     IConfiguration config,
                                     SignInManager<BankUser> signInManager,
                                     UserManager<BankUser> userManager,
                                     IOutputLogger outputLogger)
        {
            _authDb = authDb;

            _db = db;

            _config = config;
            _signInManager = signInManager;
            _userManager = userManager;
            _outputLogger = outputLogger;
        }

        public async Task<ServiceResponse> LoginWithPasscode(LoginWithPasscodeRequest request)
        {
            ServiceResponse response = new ServiceResponse();
            
            try
            {
                var user = await _userManager.FindByNameAsync(request.PhoneUid);

                if (user != null)
                {
                    var loginResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

                    if (loginResult.Succeeded)
                    {
                        var phone = _db.Phones.FirstOrDefault(p => p.PhoneUid == request.PhoneUid);

                        if (phone != null)
                        {
                            var customer = _db.Customers.Include("OnboardingStep")
                                                        .Include("Addresses")
                                                        .Include("Documents")
                                                        .Include("Phones")
                                                        .FirstOrDefault(c => c.CustomerId == phone.CustomerId);

                            if (customer != null)
                            {
                                if (customer.StatusAllowsLogin())
                                {
                                    // we need the documentId related to their address...
                                    var addressDocument = _db.Documents.FirstOrDefault(d => d.CustomerId == customer.CustomerId && d.DocumentTypeId == BankTypes.CONST_DOC_TYPE_PROOF_OF_ADDRESS.DocumentTypeId);

                                    foreach (Address addr in customer.Addresses)
                                    {              
                                        //this loop should only have an effect while we're testing with a bunch of dirty data; should be only one address per customer
                                        //maybe better tracking of addresses => documents should be done later
                                        addr.DocumentId = addressDocument.DocumentId;
                                    }

                                    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
                                    JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

                                    var claims = new[] {
                                        new Claim(JwtRegisteredClaimNames.Sub, customer.CustomerId.ToString()),
                                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                                    };

                                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));

                                    var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                                    var token = new JwtSecurityToken(
                                        _config["Tokens:Issuer"],
                                        _config["Tokens:Audience"],
                                        claims,
                                        null,
                                        DateTime.UtcNow.AddMinutes(30),
                                        credential
                                    );

                                    var result = new
                                    {
                                        sessionToken = new JwtSecurityTokenHandler().WriteToken(token),
                                        tokenExpirationDate = token.ValidTo,
                                        customer
                                    };

                                    response.Data = result;
                                }
                                else
                                {
                                    response.AddError($"Unable to login due to customer status {customer.CustomerStatusId}: { BankTypes.CustomerStatus.Find(s => s.CustomerStatusId ==customer.CustomerStatusId).Description }");
                                }
                            }
                            else
                            {
                                response.AddError("Unable to find customer for phone");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _outputLogger.Log($"{nameof(AuthenticationService)} {nameof(LoginWithPasscode)} error: {ex.Message}");

                response.AddError(ex);
            }

            return response;
        }
    }
}
