using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Core.Models
{
    [Table("PhoneConfirmationCode")]
    public class PhoneConfirmationCode
    {
        [Key]
        public int PhoneConfirmationCodeId { get; set; }

        [Required]
        public int PhoneId { get; set; }

        [Required]
        [StringLength(10)]
        public string ConfirmationCode { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime ValidUntil { get; set; }

        public DateTime? SentAt { get; set; }

        public DateTime? ConfirmedAt { get; set; }

        public DateTime? CancelledAt { get; set; }

        [StringLength(50)]
        public string CancelationReason { get; set; }

        public PhoneConfirmationCode() { }

        public static PhoneConfirmationCode Create(int phoneId, int codeLifetimeInMinutes)
        {
            var instance = new PhoneConfirmationCode
            {
                ConfirmationCode = new Random().Next(0, 999999 + 1).ToString("D6"),
                CreatedAt = DateTime.Now,
                ValidUntil = DateTime.Now.AddMinutes(codeLifetimeInMinutes),
                PhoneId = phoneId
            };

            return instance;
        }

        public void Use()
        {
            ConfirmedAt = DateTime.Now;
        }

        public void Cancel(string reason)
        {
            CancelationReason = reason;
            CancelledAt = DateTime.Now;
        }
    }
}
