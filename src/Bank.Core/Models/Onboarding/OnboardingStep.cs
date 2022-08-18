
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Core.Models
{
    [Table("OnboardingStep")]
    public class OnboardingStep
    {
        // Empty ctor for EF
        public OnboardingStep() { }

        [Key]
        public int OnboardingStepId { get; set; }

        public int? NextStepId { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Owner { get; set; }
    }
}
