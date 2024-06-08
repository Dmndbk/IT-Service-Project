using System.ComponentModel.DataAnnotations;

namespace ITService.Model
{
    public class Service
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
