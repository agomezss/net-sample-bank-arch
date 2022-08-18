using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Bank.Core.Models
{
    [Table("DocumentStatus")]
    public class DocumentStatus {

        [Required]
        public int DocumentStatusId { get; set;}
 
        [Required]
        [MaxLength(200)]
        public string Description { get; set;}
    }
}
