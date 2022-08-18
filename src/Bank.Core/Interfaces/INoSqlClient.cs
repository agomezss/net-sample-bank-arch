namespace Bank.Core.Interfaces
{
    public interface INoSqlClient
    {
        void Insert<T>(string tableName, T value) where T : class;
        void InsertNoWait<T>(string tableName, T value) where T : class;
        string GetAllSerialized(string tableName, string fieldName, string fieldValue);
    }
}
