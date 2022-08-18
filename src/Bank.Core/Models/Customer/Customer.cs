using Newtonsoft.Json;
using Bank.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Core.Models
{
    [Table("Customer")]
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        public int BirthCountryId { get; set; }

        [Required]
        public int CustomerStatusId { get; set; }

        [Required]
        public int OnboardingStepId { get; set; }

        public string AspNetUserId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [JsonIgnore]
        public DateTime RegistrationDate { get; set; }

        [Required]
        [JsonIgnore]
        public DateTime LatestUpdate { get; set; }

        public OnboardingStep OnboardingStep { get; set; }

        public virtual ICollection<Document> Documents { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }

        public virtual ICollection<Phone> Phones { get; set; }

        public Customer()
        {
            Addresses = new HashSet<Address>();
            Documents = new HashSet<Document>();
            Phones = new HashSet<Phone>();
        }

        public bool StatusAllowsLogin()
        {
            return (CustomerStatusId != BankTypes.CONST_STATUS_ACCOUNT_CANCELED.CustomerStatusId
                  && CustomerStatusId != BankTypes.CONST_STATUS_ACCOUNT_ON_HOLD.CustomerStatusId
                  && CustomerStatusId != BankTypes.CONST_STATUS_REJECTED.CustomerStatusId
                  && CustomerStatusId != BankTypes.CONST_STATUS_SUSPENDED.CustomerStatusId);
        }

        public string GetFirstName()
        {
            if (!string.IsNullOrEmpty(Name))
            {
                if (Name.Contains(' '))
                {
                    return Name.Split(' ')[0];
                }
                else
                {
                    return Name;
                }
            }

            return "";
        }
    }
}
