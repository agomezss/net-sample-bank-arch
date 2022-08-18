namespace Bank.Core.Exceptions
{
    public class StorageException: CoreException

    {
        public int HttpErrorCode { get; set; }

        public StorageException(string message = "Cloud Storage Exception", string errorCode = null) : base(message, errorCode)
        {

        }

        public void SetErrorCode(int errorCode)
        {
            HttpErrorCode = errorCode;
        }

    }
}