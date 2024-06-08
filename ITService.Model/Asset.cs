using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITService.Model
{
    public class Asset
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string? Location { get; set; }

        [StringLength(100)]
        public string? SerialNumber { get; set; }
        [MaxLength]
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Price { get; set; }
        public int? AssetTypeId { get; set; }
        public AssetType? AssetType { get; set; }
        public int? CompanyId { get; set; }
        public Company? Company { get; set; }
    }
}
