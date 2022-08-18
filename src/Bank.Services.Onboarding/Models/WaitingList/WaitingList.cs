using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Services.Onboarding.Models.WaitingList
{
    [Table("WaitingList")]
    public class WaitingList
    {
        [Key]
        [Required]
        public int WaitingListId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(1)]
        public string Active { get; set; }

        [Required]
        public int DailyCapacity { get; set; }

    }
}
