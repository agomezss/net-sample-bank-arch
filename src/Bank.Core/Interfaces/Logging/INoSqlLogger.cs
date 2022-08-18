namespace Bank.Core.Interfaces
{
    public interface INoSqlLogger
    {
        INoSqlLogger Log<T>(string tableName, T data) where T : class;
    }
}
