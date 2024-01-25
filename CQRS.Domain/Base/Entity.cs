using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Domain.Base
{
    public class Entity<TKey>
    {
        [Required]
        public TKey Id { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public Guid CreatorPerson { get; set; }

        public DateTime? LastUpdateDate { get; set; }

        public bool Deleted { get; set; } = false;
    }
}
