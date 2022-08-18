namespace Bank.Core.Exceptions
{
    public class NoSqlException: CoreException

    {
        public int HttpErrorCode { get; set; }

        public NoSqlException(string message = "NoSql Exception", string errorCode = null) : base(message, errorCode)
        {

        }

        public void SetErrorCode(int errorCode)
        {
            HttpErrorCode = errorCode;
        }
    }
}