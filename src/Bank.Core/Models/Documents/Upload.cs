using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Bank.Core.Models
{
    [Table("Upload")]
    public class Upload
    {
        [Required]
        public int UploadId { get; set;}
 
        [Required]
        public int UploadTypeId { get; set;}
 
        [Required]
        public int CustomerId { get; set;}
 
        [Required]
        public int DocumentId { get; set;}
 
        [Required]
        public string DocumentAwsReference { get; set;}
 
        public string DocumentUrl { get; set;}
 
        public DateTime EventDateTime { get; set;}
 
        public Upload()
        {
            this.EventDateTime = DateTime.Now;
        }
    }
}
