using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Core.Models
{
    [Table("Document")]
    public class Document
    {
        // Empty ctor for EF
        public Document() { }

        [Key]
        public int DocumentId { get; set; }

        public int DocumentStatusId { get; set; }
        
        [Required]
        public int DocumentTypeId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(50)]
        public string DocumentNumber { get; set; }

        public string DocumentDetails { get; set; }
    }
}
