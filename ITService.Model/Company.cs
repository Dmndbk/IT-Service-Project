using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace ITService.Model
{
    public class Company
    {
        public Company()
        {
            Clients = new Collection<Client>();
        }
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(150)]
        public string? Description { get; set; }
        public ICollection<Client> Clients { get; set; }
    }

}
