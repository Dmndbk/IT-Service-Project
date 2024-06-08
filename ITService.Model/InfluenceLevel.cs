using System.ComponentModel.DataAnnotations;

namespace ITService.Model
{
    public class InfluenceLevel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
