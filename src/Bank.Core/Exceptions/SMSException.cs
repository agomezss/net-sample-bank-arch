namespace Bank.Core.Exceptions
{
    public class SMSException: CoreException

    {
        public int HttpErrorCode { get; set; }

        public SMSException(string message = "SMS Sending Exception", string errorCode = null) : base(message, errorCode)
        {

        }

        public void SetErrorCode(int errorCode)
        {
            HttpErrorCode = errorCode;
        }
    }
}