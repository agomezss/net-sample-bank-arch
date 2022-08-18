namespace Bank.Core.Exceptions
{
    public class EmailException: CoreException

    {
        public int HttpErrorCode { get; set; }

        public EmailException(string message = "Email Sending Exception", string errorCode = null) : base(message, errorCode)
        {

        }

        public void SetErrorCode(int errorCode)
        {
            HttpErrorCode = errorCode;
        }
    }
}