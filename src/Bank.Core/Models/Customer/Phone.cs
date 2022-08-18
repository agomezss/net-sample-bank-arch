
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Core.Models
{
    [Table("Phone")]
    public class Phone
    {
        [Key]
        public int PhoneId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(200)]
        public string PhoneUid { get; set; }

        [Required]
        public string CountryCode { get; set; }

        [Required]
        public string AreaCode { get; set; }

        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(1)]
        public string Confirmed { get; set; }

        public virtual ICollection<PhoneConfirmationCode> ConfirmationCodes { get; set; }

        public Phone()
        {
            Confirmed = "N";
        }

        public override string ToString()
        {
            return $"+{CountryCode}{AreaCode}{PhoneNumber}";
        }
    }
}
