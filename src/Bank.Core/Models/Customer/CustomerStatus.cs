using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Core.Models
{
    [Table("CustomerStatus")]
    public class CustomerStatus
    {
        // Empty ctor for EF
        protected CustomerStatus() { }

        [Key]
        public int CustomerStatusId { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        [JsonIgnore]
        public Customer Customer { get; set; }
    }
}
