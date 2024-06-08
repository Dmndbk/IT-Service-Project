using System.ComponentModel.DataAnnotations;

namespace ITService.Model
{
    public class Problem
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
        [StringLength(100)]
        public string? MainCause { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

        //Текущий уровень влияния
        [Required]
        public int? CurrentInfluenceLevelId { get; set; }
        public InfluenceLevel? CurrentInfluenceLevel { get; set; }
        public int? CurrentStatusId { get; set; }
        public Status? CurrentStatus { get; set; }
        public int? RelatedEmployeeId { get; set; }
        public Employee? RelatedEmployee { get; set; }

    }
}
