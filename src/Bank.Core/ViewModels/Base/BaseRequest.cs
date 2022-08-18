using System;

namespace Bank.Core.ViewModels
{
    public class BaseRequest
    {
        public string PhoneUid { get; set; }
        public DateTime PhoneLocalTime { get; set; }
    }
}
