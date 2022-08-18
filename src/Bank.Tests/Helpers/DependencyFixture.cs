using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Bank.Core.Context;
using Bank.Core.Models;
using Bank.Infra.CrossCutting.IoC;
using Bank.Services.Authentication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bank.Tests.Helpers
{
    public class DependencyFixture
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Hosting { get; }

        public IServiceCollection Services { get; set; }

        public DependencyFixture()
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .AddEnvironmentVariables();

            var config = builder.Build();
            Configuration = config;

            var connectionString = config.GetConnectionString("BankDbConnection");
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<OnboardingDbContext>(options => options.UseSqlServer(connectionString),
                    ServiceLifetime.Transient);

            serviceCollection.AddDbContext<BankDbContext>(options => options.UseSqlServer(connectionString),
                    ServiceLifetime.Transient);

            var hostEnvironment = new HostEnvironment
            {
            };

            Hosting = hostEnvironment;

            serviceCollection.AddDbContext<AuthDbContext>(cfg =>
            {
                cfg.UseSqlServer(connectionString, m => m.MigrationsAssembly("BankApi"));
            });

            serviceCollection.AddIdentity<BankUser, IdentityRole>(cfg =>
             {
                 cfg.User.RequireUniqueEmail = true;
                 cfg.Password.RequireDigit = true;
                 cfg.Password.RequiredLength = 6;
                 cfg.Password.RequiredUniqueChars = 0;
                 cfg.Password.RequireLowercase = false;
                 cfg.Password.RequireNonAlphanumeric = false;
                 cfg.Password.RequireUppercase = false;
             })
             .AddEntityFrameworkStores<AuthDbContext>();

            serviceCollection.AddSingleton(provider => Configuration);
            serviceCollection.AddSingleton(provider => Hosting);
            NativeInjectorBootStrapper.RegisterServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        public ServiceProvider ServiceProvider { get; private set; }
    }

}
