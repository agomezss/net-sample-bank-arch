using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Core.Models
{
    [Table("DocumentType")]
    public class DocumentType
    {
        // Empty ctor for EF
        protected DocumentType() { }

        [Key]
        public int DocumentTypeId { get; set; }

        [Required]
        public int CountryId { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }
    }
}
