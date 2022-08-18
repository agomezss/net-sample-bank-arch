using Bank.Core.Interfaces;

namespace Bank.Infra.Logging.NoSqlLogger
{
    public class NoSqlLogger : INoSqlLogger
    {
        private readonly INoSqlClient _noSqlClient;

        public NoSqlLogger(INoSqlClient noSqlClient)
        {
            _noSqlClient = noSqlClient;
        }

        public INoSqlLogger Log<T>(string tableName, T data) where T : class
        {
            _noSqlClient.Insert(tableName, data);
            return this;
        }
    }
}
