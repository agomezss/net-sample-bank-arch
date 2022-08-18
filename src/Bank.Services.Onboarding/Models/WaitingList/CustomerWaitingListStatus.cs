using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Services.Onboarding.Models.WaitingList
{
    [Table("CustomerWaitingListStatus")]
    public class CustomerWaitingListStatus
    {
        [Required]
        public int ProspectId { get; set; }

        [Required]
        public int StatusId { get; set; }

        [Timestamp]
        public byte[] EventTimestamp { get; set; }

    }
}
