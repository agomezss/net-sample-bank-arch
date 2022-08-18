using Bank.Core.Models.Storage;
using System.Collections.Generic;
using System.IO;

namespace Bank.Core.Interfaces
{
    public interface IStorageClient
    {
        string GetWorkingBucketName();
        IStorageClient WithBucket(string bucket);
        List<StorageItem> List(string key = null);
        byte[] Get(string key);
        bool Put(string key, Stream stream);
        bool Delete(string key);
    }
}
