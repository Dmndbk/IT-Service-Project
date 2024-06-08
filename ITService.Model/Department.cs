using System.ComponentModel.DataAnnotations;

namespace ITService.Model
{
    public class Department
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(150)]
        public string? Description { get; set; }
    }
}
