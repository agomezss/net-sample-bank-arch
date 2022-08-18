using Bank.Core.Models;
using System.Net.Mail;

namespace Bank.Core.Interfaces
{
    public interface IEmailSender
    {
        bool SendEmail(MailMessage mailMessage);
    }
}
