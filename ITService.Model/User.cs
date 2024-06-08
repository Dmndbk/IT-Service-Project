using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ITService.Model
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string Login { get; set; }
        [Required]
        [StringLength(50)]
        [PasswordPropertyText]
        public string Password { get; set; }
        public int? UserRoleId { get; set; }
        public UserRole? UserRole { get; set; }
    }
}
