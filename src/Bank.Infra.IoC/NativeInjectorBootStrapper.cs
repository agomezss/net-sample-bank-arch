using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Bank.Core.Interfaces;
using Bank.Infra.Data.NoSql;
using Bank.Services.Onboarding.Interfaces;
using Bank.Services.Onboarding;
using Bank.Infra.Logging.ConsoleLog;
using Bank.Infra.Logging.NoSqlLogger;
using Bank.Infra.Logging;
using Bank.Infra.Messaging.Email;
using Bank.Infra.Messaging.SMS;
using Bank.Core.Context;
using Bank.Services.Authentication;
using Bank.Services.Authentication.Interfaces;
using Bank.Services.Authentication.Services;
using Microsoft.AspNetCore.Identity;
using Bank.Core.Models;

namespace Bank.Infra.CrossCutting.IoC
{
    /// <summary>
    /// Dependency Injection:
    /// 
    /// General Use:
    /// 
    /// Singleton -> Same instance for all application and all requests
    /// Scope -> Same instance for all request lifecycle
    /// Transient -> Always a new instance.
    /// </summary>
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // ASP.NET HttpContext dependency
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<UserManager<BankUser>>();

            // Application
            services.AddScoped<IProspectService, ProspectService>();
            services.AddScoped<IPhoneConfirmationCodeService, PhoneConfirmationService>();
            
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IPhoneService, PhoneService>();
            services.AddScoped<IUploadService, UploadService>();
            services.AddScoped<IWaitingListService, WaitingListService>();

            // Infra - Data
            services.AddScoped<BankDbContext>();
            services.AddScoped<OnboardingDbContext>();
            services.AddScoped<AuthDbContext>();

            // Infra - Data EventSourcing
            services.AddScoped<INoSqlClient, AwsDynamoDbClient>();

            // Infra - Logging
            services.AddTransient<IOutputLogger, ConsoleLogger>();
            services.AddTransient<INoSqlLogger, NoSqlLogger>();
            services.AddTransient<IActivityLogger, ExtendedActivityLogger>();

            // Infra - Messaging
            services.AddTransient<IEmailSender, EmailSmtpClient>();
            services.AddTransient<ISmsSender, TwilioMobileClient>();
        }
    }
}