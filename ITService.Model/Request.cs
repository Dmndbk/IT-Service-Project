using System.ComponentModel.DataAnnotations;

namespace ITService.Model
{
    public class Request
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
        public DateTime CreationDate { get; set; }
        public DateTime? CloseDate { get; set; }
        //Контактное лицо
        public int? RelatedClientId { get; set; }
        public Client? RelatedClient { get; set; }

        //Связанные активы
        public int? RelatedAssetId { get; set; }
        public Asset? RelatedAsset { get; set; }

        //Ответственный сотрулник (один)
        public int? RelatedEmployeeId { get; set; }
        public Employee? RelatedEmployee { get; set; }

        //Статус заявки
        public int? CurrentStatusId { get; set; }
        public Status? CurrentStatus { get; set; }

        //Услуга
        public int? ServiceId { get; set; }
        public Service? Service { get; set; }

        public int? AssessmentId { get; set; }
        public Assessment? Assessment { get; set;}

        [StringLength(50)]
        public string? Comment { get; set; }
    }
}
