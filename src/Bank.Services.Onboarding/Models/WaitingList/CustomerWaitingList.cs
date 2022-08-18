using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Services.Onboarding.Models.WaitingList
{
    [Table("CustomerWaitingList")]
    public class CustomerWaitingList
    {
        [Required]
        public int ProspectId { get; set; }

        [Required]
        public int WaitingListId { get; set; }

        [Required]
        public int StatusId { get; set; }

        [Required]
        public int PositionInQueue { get; set; }

        [Timestamp]
        public byte[] EventTimestamp { get; set; }

        public void ChangeStatus(int statusId)
        {
            StatusId = statusId;
        }
    }
}
