using CQRS.Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Domain.Entities
{
    public class Product : Entity<Guid>
    {
        [DisplayName("Title")]
        [MaxLength(150, ErrorMessage = "{0} could not be more than {1} characters")]
        [Required(ErrorMessage = "{0} is required")]
        public string Title { get; set; }

        [DisplayName("Produce Date")]
        [Required(ErrorMessage = "{0} is required")]
        public DateTime ProduceDate { get; set; } = DateTime.UtcNow;

        [DisplayName("Manufacture Phone")]
        [MaxLength(11, ErrorMessage = "{0} could not be more than {1} characters")]
        [MinLength(8, ErrorMessage = "{0} could not be more than {2} characters")]
        [Required(ErrorMessage = "{0} is required")]
        public string ManufacturePhone { get; set; }

        [DisplayName("Manufacture Email")]
        [Required(ErrorMessage = "{0} is required")]
        [EmailAddress]
        public string ManufactureEmail { get; set; }

        [DisplayName("Is Available")]
        [Required(ErrorMessage = "{0} is required")]
        public bool IsAvailable { get; set; } = false;
    }
}
