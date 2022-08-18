using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Core.Models
{
    [Table("Address")]
    public class Address 
    {
        // Empty ctor for EF
        protected Address(){}

        [Key]
        public int AddressId { get; set; }

        public int CustomerId { get; set; }

        public int CountryId { get; set; }

        [NotMapped]
        public int DocumentId { get; set; }
        // this is only to recover the documentId when the customer sent the address but not the address proof yet

        [JsonIgnore]
        public Country Country { get; set; }

        [Required]
        [StringLength(12)]
        public string ZipCode { get; set; }

        [Required]
        [StringLength(100)]
        public string City { get; set; }

        [Required]
        [StringLength(100)]
        public string District { get; set; }

        [Required]
        [StringLength(200)]
        [Column("address")]
        public string AddressName { get; set; }

        [Required]
        [StringLength(20)]
        public string AddressNumber { get; set; }

        [StringLength(100)]
        public string Complement { get; set; }
    }
}
