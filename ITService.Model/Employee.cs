using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace ITService.Model
{
    public class Employee
    {
        public Employee()
        {
            Requests = new Collection<Request>();
            Objectives = new Collection<Objective>();
            Changes = new Collection<Change>();
        }
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string Patronymic { get; set; }

        [StringLength(50)]
        public string Position { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; } 
        public ICollection<Request> Requests { get; set; }
        public ICollection<Objective> Objectives { get; set; }
        public ICollection<Change> Changes { get; set; }
    }
}
