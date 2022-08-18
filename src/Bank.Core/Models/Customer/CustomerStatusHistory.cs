using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Core.Models
{
    [Table("CustomerStatusHistory")]
    public class CustomerStatusHistory
    {
        // Empty ctor for EF
        protected CustomerStatusHistory() { }

        [Required]
        public int CustomerStatusId { get; set; }

        [Required]
        public string CustomerId { get; set; }

        [Required]
        [JsonIgnore]
        public DateTime EventTimestamp { get; set; }
    }
}
