//using Bank.Infra.CrossCutting.Identity.Authorization;
//using System;

//namespace Bank.Services.Api.Configurations
//{
//    public static class AuthorizationPolicy
//    {
//        public static Action<Microsoft.AspNetCore.Authorization.AuthorizationOptions> GetPolicies()
//        {
//            return options =>
//            {
//                // Equinox
//                options.AddPolicy("CanWriteCustomerData", policy => policy.Requirements.Add(new ClaimRequirement("Customers", "Write")));
//                options.AddPolicy("CanRemoveCustomerData", policy => policy.Requirements.Add(new ClaimRequirement("Customers", "Remove")));

//                // Bank
//            };
//        }
//    }
//}
