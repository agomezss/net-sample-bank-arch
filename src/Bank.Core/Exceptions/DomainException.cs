namespace Bank.Core.Exceptions
{
    public class DomainException: CoreException
    {
        public DomainException(string message = null, string errorCode = null) : base(message, errorCode)
        {

        }
    }
}