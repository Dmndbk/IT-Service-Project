using System.ComponentModel.DataAnnotations;

namespace ITService.Model
{
    public class Objective
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Theme { get; set; }

        [Required]
        [MaxLength]
        public string Description { get; set; }

        [MaxLength]
        public string? SolutionDescription { get; set; }
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        //Ответственный сотрулник (один)
        public int? ResponsibleEmployeeId { get; set; }
        public Employee ResponsibleEmployee { get; set; }

        public int? CurrentStatusId { get; set; }
        public Status? CurrentStatus { get; set; }
    }
}
