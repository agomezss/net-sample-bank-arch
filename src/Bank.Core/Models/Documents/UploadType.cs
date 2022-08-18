using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Bank.Core.Models
{
    [Table("UploadType")]
    public class UploadType
    {
        [Required]
        public int UploadTypeId { get; set;}
 
        [Required]
        [MaxLength(200)]
        public string Description { get; set;}
 
    }
}
