using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Core.Models
{
  [Table("Country")]
  public class Country
  {
    // Empty ctor for EF
    protected Country() { }

    [Key]
    public int CountryId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [StringLength(10)]
    public string Abbreviation { get; set; }

    [Required]
    public int PhoneCode { get; set; }
  }
}
