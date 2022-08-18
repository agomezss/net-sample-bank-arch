using Bank.Core.Interfaces;
using Bank.Core.Models;
using System.IO;
using System.Net.Mail;
using System.Text;

namespace Bank.POC.Test
{
    public class TestEmailSending
    {
        private readonly IEmailSender _sender;

        public TestEmailSending(IEmailSender sender)
        {
            _sender = sender;
        }

        static byte[] GetDataTxt()
        {
            string s = "this is some example test";
            byte[] data = Encoding.ASCII.GetBytes(s);
            return data;
        }

        public void TestComplete()
        {
            var mailMessage = new MailMessage
            {
                Body = "Test Mail Send Complete With Attachment",
                IsBodyHtml = false,
                Subject = "Test"
            };

            mailMessage.To.Add(new MailAddress("agomez@Banknvest.com"));

            // Add txt file as attachment
            MemoryStream ms = new MemoryStream(GetDataTxt());
            mailMessage.Attachments.Add(new Attachment(ms, "example.txt", "text/plain"));

            _sender.SendEmail(mailMessage);
        }
    }
}
