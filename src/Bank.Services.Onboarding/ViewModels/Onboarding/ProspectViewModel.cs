using Bank.Core.Extensions;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bank.Services.Onboarding.ViewModels
{
    public class ProspectViewModel
    {
        public int ProspectId { get; set; }

        [StringLength(100)]
        [DisplayName("DocumentNumber")]
        public string DocumentNumber { get; set; }

        [StringLength(100)]
        [DisplayName("Name")]
        public string Name { get; set; }

        [StringLength(100)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid e-mail address")]
        [DisplayName("E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The birth date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date, ErrorMessage = "Invalid birth date")]
        [MinimumAge(18, ErrorMessage = "You have to be over eighteen")]
        [DisplayName("Birth Date")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "The country is required")]
        [DisplayName("Country")]
        public int CountryId { get; set; }

        [DisplayName("Area Code")]
        [StringLength(5)]
        public string AreaCode { get; set; }

        [Required(ErrorMessage = "The phone number is required")]
        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage = "Invalid phone number")] 
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "The password is required")]
        [DataType(DataType.Password)]
        [MinLength(6)]
        [MaxLength(6)]
        [DisplayName("Password")]
        public string Password { get; set; }
        
        [Required]
        public string PhoneUid { get; set; }
    }
}
