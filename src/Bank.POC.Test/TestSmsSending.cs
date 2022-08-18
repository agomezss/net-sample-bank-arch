using Bank.Core.Interfaces;

namespace Bank.POC.Test
{
    public class TestSmsSending
    {
        private readonly ISmsSender _sender;

        public TestSmsSending(ISmsSender sender)
        {
            _sender = sender;
        }

        public void Test()
        {
            var message = "Test Bank Twilio";
            var number = "+5511947266988";
            _sender.SendMessage(message, number);
        }
    }
}
