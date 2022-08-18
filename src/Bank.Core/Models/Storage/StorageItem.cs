using System;

namespace Bank.Core.Models.Storage
{
    public class StorageItem
    {
        public string Tag { get; set; }
        public string StorageName { get; set; }
        public string Key { get; set; }
        public DateTime LastModified { get; set; }
        public string OwnerName { get; set; }
        public string OwnerId { get; set; }
        public long Size { get; set; }
    }
}
