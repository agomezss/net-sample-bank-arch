using Newtonsoft.Json;
using Bank.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Services.Onboarding.Models
{
    [Table("Prospect")]
    public class Prospect
    {
        [Key]
        public int ProspectId { get; set; }

        [Required]
        public int DocumentTypeId { get; set; }

        public virtual DocumentType DocumentType { get; set; }

        [Required]
        [StringLength(1)]
        public string ProspectStatus { get; set; }

        [Required]
        [StringLength(100)]
        public string DocumentNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public int CountryId { get; set; }

        public virtual Country Country {get; set;}

        [Required]
        [StringLength(5)]
        public string AreaCode { get; set; }

        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }
        
        [Required]
        public DateTime CreationDate { get; set; }

        public string AspNetUserId { get; set; }
    }
}
