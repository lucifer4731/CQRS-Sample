using CQRS.Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Domain.Entities
{
    public class Product : Entity<Guid>
    {
        [Required]
        [MaxLength(150)]
        [MinLength(2)]
        public string Title { get; set; }

        [Required]
        public DateTime ProduceDate { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(11)]
        [MinLength(8)]
        public string ManufacturePhone { get; set; }

        [Required]
        [EmailAddress]
        public string ManufactureEmail { get; set; }

        [Required]
        public bool IsAvailable { get; set; } = false;
    }
}
