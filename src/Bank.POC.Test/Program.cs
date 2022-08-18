using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Bank.Core.Interfaces;
using Bank.Infra.Logging.ConsoleLog;
using Bank.Infra.Messaging.Email;
using Bank.Infra.Messaging.SMS;

namespace Bank.POC.Test
{

    class Program
    {
        public static ILoggerFactory LoggerFactory;
        public static IConfiguration Configuration;

        static void Main(string[] args)
        {
            Configuration = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json")
                                .Build();

            var services = new ServiceCollection();
            services.AddSingleton(provider => Configuration);
            services.AddSingleton(logger => LoggerFactory);
            services.AddTransient<IOutputLogger, ConsoleLogger>();
            services.AddTransient<IEmailSender, EmailSmtpClient>();
            services.AddTransient<ISmsSender, TwilioMobileClient>();

            var sp = services.BuildServiceProvider();

#pragma warning disable CS0618 // Type or member is obsolete
            LoggerFactory = new LoggerFactory()
                           .AddConsole(Configuration.GetSection("Logging"));
            //               .AddDebug();
#pragma warning restore CS0618 // Type or member is obsolete

            //IEmailSender emailSender = sp.GetRequiredService<IEmailSender>();
            //new TestEmailSending(emailSender).TestComplete();

            ISmsSender smsSender = sp.GetRequiredService<ISmsSender>();
            new TestSmsSending(smsSender).Test();

            Console.ReadLine();
        }
    }
}
