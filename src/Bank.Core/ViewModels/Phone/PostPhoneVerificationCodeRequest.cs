using System.ComponentModel.DataAnnotations;

namespace Bank.Core.ViewModels
{
    public class PostPhoneVerificationCodeRequest : BaseRequest
    {
        [Required]
        [StringLength(6)]
        public string VerificationCode { get; set; }
        public PostPhoneVerificationCodeRequest() : base() { }
    }
}
