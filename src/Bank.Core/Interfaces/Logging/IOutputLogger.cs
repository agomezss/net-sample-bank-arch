namespace Bank.Core.Interfaces
{
    public interface IOutputLogger
    {
        IOutputLogger Log<T>(T data) where T : class;
    }
}
