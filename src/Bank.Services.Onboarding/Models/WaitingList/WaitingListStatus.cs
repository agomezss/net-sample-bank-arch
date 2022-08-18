using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Services.Onboarding.Models.WaitingList
{
    [Table("WaitingListStatus")]
    public class WaitingListStatus
    {
        [Key]
        [Required]
        public int StatusId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Description { get; set; }

    }
}
