
using System.Collections.Generic;
using System.Security.Claims;

namespace Bank.Core
{
    public interface IUser
    {
        string Name { get; }
        bool IsAuthenticated();
        IEnumerable<Claim> GetClaimsIdentity();
    }
}
