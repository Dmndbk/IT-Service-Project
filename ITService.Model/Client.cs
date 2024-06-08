using System.ComponentModel.DataAnnotations;

namespace ITService.Model
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string Patronymic { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }
        
        public int? CompanyId { get; set; }
        public Company? Company { get; set;}
        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}
