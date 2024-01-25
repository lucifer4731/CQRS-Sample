using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [DisplayName("Description")]
        [MaxLength(500, ErrorMessage = "{0 could not be more than 500 characters}")]
        public string? Description { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        public DateTime? LastUpdateDate { get; set; }

        public bool Deleted { get; set; } = false;
    }
}
