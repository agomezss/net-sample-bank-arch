using Microsoft.Extensions.Configuration;
using Bank.Core.Interfaces;
using System;
using System.Net;
using System.Net.Mail;

namespace Bank.Infra.Messaging.Email
{
    /// <summary>
    /// Implement a SMTP client that allows e-mail sending.
    /// </summary>
    public class EmailSmtpClient : IEmailSender
    {
        private readonly IConfiguration _config;
        private readonly IOutputLogger _outputLogger;

        private readonly string _senderAddress;
        private readonly string _senderName;
        private readonly string _SMTPServer;
        private readonly int _SMTPPort;
        private readonly string _SMTPUser;
        private readonly string _SMTPPassword;
        private readonly bool _SMTPUseSSL;

        public EmailSmtpClient(IConfiguration config, IOutputLogger outputLogger)
        {
            _config = config;
            _outputLogger = outputLogger;
            _senderAddress = _config["Email:SenderEmail"];
            _senderName = _config["Email:SenderName"];
            _SMTPServer = _config["Email:SMTPServer"];
            _SMTPPort = int.Parse(_config["Email:SMTPPort"]);
            _SMTPUser = _config["Email:SMTPUser"];
            _SMTPPassword = _config["Email:SMTPPassword"];
            _SMTPUseSSL = bool.Parse(_config["Email:SMTPUseSSL"]);
        }

        private string GetRecipients(MailMessage message)
        {
            return message.To?.ToString();
        }

        public bool SendEmail(MailMessage mailMessage)
        {
            using (var client = new SmtpClient(_SMTPServer, _SMTPPort))
            {
                if (mailMessage.From == null || string.IsNullOrEmpty(mailMessage.From.Address))
                    mailMessage.From = new MailAddress(_senderAddress, _senderName);

                client.Credentials = new NetworkCredential(_SMTPUser, _SMTPPassword);
                client.EnableSsl = _SMTPUseSSL;

                try
                {
                    _outputLogger.Log($"Sending email using SMTP - Recipient(s): {GetRecipients(mailMessage)}, Subject: {mailMessage.Subject}");
                    client.Send(mailMessage);
                    _outputLogger.Log($"The email was sent successfully. Recipient(s): {GetRecipients(mailMessage)}, Subject: {mailMessage.Subject}");
                    return true;

                }
                catch (Exception ex)
                {
                    _outputLogger.Log($"The email was not sent. Recipient(s): {GetRecipients(mailMessage)}, Subject: {mailMessage.Subject}");
                    _outputLogger.Log($"Recipient(s): {GetRecipients(mailMessage)}, Subject: {mailMessage.Subject}. Error message: " + ex.Message);
                    return false;
                }
            }
        }
    }
}
