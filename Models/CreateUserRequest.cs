using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Message_Service.Models
{
    public class CreateUserRequest
    {
        // Properties for creating user.
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
