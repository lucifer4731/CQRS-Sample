using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Application.Dto
{
    public class AuthenticateDto
    {
        public string Token { get; set; }
        public string UserName { get; set; }
    }
}
