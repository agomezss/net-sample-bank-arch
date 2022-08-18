using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Api.Logging
{
    [Serializable]
    public class RequestResponseData
    {
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Path { get; set; }
        public string QueryString { get; set; }
        public string RequestHeaders { get; set; }
        public string RequestBody { get; set; }
        public int StatusCode { get; set; }
        public string ResponseHeaders { get; set; }
        public string ResponseBody { get; set; }
        public string User { get; set; }
    }
}
