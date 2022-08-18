using Amazon;
using Microsoft.Extensions.Configuration;
using System;

namespace Bank.Services.Api
{
    public static class BankApiSettings
    {
        public static bool IsProduction = false;
        public static bool UseLambdaResultObjectResponse = false;
        public static RegionEndpoint CustomerDocumentsBucketRegion = null;

        public static void Configure(IConfiguration config)
        {
            IsProduction = Convert.ToBoolean(config["BankSettings:InProduction"].ToLower());
            UseLambdaResultObjectResponse = Convert.ToBoolean(config["BankSettings:UseLambdaResultObjectResponse"].ToLower());

            string region = config["BankSettings:CustomerDocumentsBucketRegion"];

            if (!string.IsNullOrEmpty(region))
            {
                CustomerDocumentsBucketRegion = RegionEndpoint.GetBySystemName(region);
            }
        }
    }
}
