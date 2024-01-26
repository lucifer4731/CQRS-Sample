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
    public class User : Entity<Guid>
    {
        [DisplayName("Full Name")]
        [MaxLength(150, ErrorMessage = "{0} could not be more than {1} characters")]
        [Required(ErrorMessage = "{0} is required")]
        public string FullName { get; set; }

        [DisplayName("UserName")]
        [MaxLength(150, ErrorMessage = "{0} could not be more than {1} characters")]
        [Required(ErrorMessage = "{0} is required")]
        public string UserName { get; set; }

        [DisplayName("Password")]
        [Required(ErrorMessage = "{0} is required")]
        public string Password { get; set; }
    }
}
