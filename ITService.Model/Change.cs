using System.ComponentModel.DataAnnotations;

namespace ITService.Model
{
    public class Change
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Theme { get; set;}

        [Required]
        [MaxLength]
        public string Description { get; set;}

        [MaxLength]
        public string? SolutionDescription { get; set; }

        [MaxLength]
        public string? ImplementationPlan { get; set;}

        [MaxLength]
        public string? RollbackPlan { get; set; }
        
        [Required]
        public DateTime EndDate { get; set; }

        //Ответственный сотрулник (один)
        public int? RelatedEmployeeId { get; set; }
        public Employee? RelatedEmployee { get; set; }

        public int? RelatedServiceId { get; set; }
        public Service? RelatedService { get; set; }
        public int? CurrentStatusId { get; set; }
        public Status? CurrentStatus { get; set; }
    }
}
