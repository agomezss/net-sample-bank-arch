using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Security.Claims;

namespace Bank.Core.Models
{
    public class BankUser : IdentityUser
    {
        // Empty ctor for EF
        public BankUser() { }

        public string Name { get; set; }
        
        public static int GetCustomerIdFor(System.Security.Principal.IPrincipal user)
        {

            var claimData = ((ClaimsIdentity)user.Identity).Claims.FirstOrDefault(c => c.Type == "sub");

            if (claimData == null)
            {

                //MS claims conversor BS happening here, this is a workaround for when this line:
                // JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();
                //doesn't do the trick somehow. moving on.

                claimData = ((ClaimsIdentity)user.Identity).Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            }

            return Convert.ToInt32(claimData.Value);
        }

        public static string GetPhoneUidFor(System.Security.Principal.IPrincipal user)
        {

            var claimData = ((ClaimsIdentity)user.Identity).Claims.FirstOrDefault(c => c.Type == "unique_name");

            if (claimData == null)
            {

                //MS claims conversor BS happening here, this is a workaround for when this line:
                // JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();
                //doesn't do the trick somehow. moving on.

                claimData = ((ClaimsIdentity)user.Identity).Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
            }

            return claimData.Value;
        }

        public static string GetRoleFor(System.Security.Principal.IPrincipal user)
        {

            var u = ((ClaimsIdentity)user.Identity);

            return u.Claims.First(c => c.Type == u.RoleClaimType).Value;
        }

        public static bool IsAdmin()
        {
            return false; // HttpContext.Current.User.IsInRole(ApplicationRoleNames.Admin.ToString());
        }
    }
}