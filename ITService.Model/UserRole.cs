using System.ComponentModel.DataAnnotations;

namespace ITService.Model
{
    public class UserRole
    {
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string Name { get; set; }
    }
}
