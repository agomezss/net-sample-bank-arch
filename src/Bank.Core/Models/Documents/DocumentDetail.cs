using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Bank.Core.Models
{
    [Table("DocumentDetail")]
    public class DocumentDetail
    {
        [Required]
        public int DocumentDetailId { get; set;}
 
        [Required]
        public int DocumentId { get; set;}
 
        [Required]
        public int DocumentStatusId { get; set;}
 
        [Required]
        [MaxLength(200)]
        public string Detail { get; set;}
 
        [Required]
        public DateTime EventDateTime { get; set;}
 
        public DocumentDetail()
        {
            this.EventDateTime = DateTime.Now;
        }
    }
}
