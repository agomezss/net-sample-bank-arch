using Bank.Core.Interfaces;
using System;

namespace Bank.Infra.Logging.ConsoleLog
{
    public class ConsoleLogger : IOutputLogger
    {
        public IOutputLogger Log<T>(T data) where T : class
        {
            Console.WriteLine(data);
            return this;
        }
    }
}
