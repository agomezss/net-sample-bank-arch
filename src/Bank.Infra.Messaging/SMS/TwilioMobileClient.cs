using Microsoft.Extensions.Configuration;
using Bank.Core.Interfaces;
using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Bank.Infra.Messaging.SMS
{
    public class TwilioMobileClient : ISmsSender
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _fromPhoneNumber;
        private readonly bool _enabled;

        public TwilioMobileClient(IConfiguration config)
        {
            _accountSid = config["Twilio:AccountSid"];
            _authToken = config["Twilio:AuthToken"];
            _fromPhoneNumber = config["Twilio:FromNumber"];
            _enabled = Convert.ToBoolean(config["Twilio:Enabled"].ToLower());
        }

        public string SendMessage(string messageBody, string sendNumber)
        {
            try
            {
                TwilioClient.Init(_accountSid, _authToken);

                if (!_enabled)
                    throw new Exception("Twilio is disabled");

                var message = MessageResource.Create(
                    body: messageBody,
                    from: new Twilio.Types.PhoneNumber(_fromPhoneNumber),
                    to: new Twilio.Types.PhoneNumber(sendNumber)
                );

                return message.Sid;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
