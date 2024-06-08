using System.ComponentModel.DataAnnotations;
namespace ITService.Model
{
    public class Assessment
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        
        [Required]
        public int Mark { get; set; }
    }
}
