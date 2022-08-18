
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Core.Models
{
    [Table("OnboardingStepHistory")]
    public class OnboardingStepHistory
    {
        // Empty ctor for EF
        public OnboardingStepHistory() { }

        [Required]
        public int OnboardingStepId { get; set; }

        [Required]
        public int CustomerId { get; set; }
    }
}
