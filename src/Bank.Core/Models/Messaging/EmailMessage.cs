using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Core.Models
{
    public class EmailMessage
    {
        public string RecipientEmail { get; set; }
        public string RecipientName { get; set; }
        public string Subject { get; set; }
        public string HtmlBody { get; set; }
        public string TextBody { get; set; }
    }
}
