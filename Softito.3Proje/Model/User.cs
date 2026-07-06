using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class User : IdentityUser<int>
    {
        public string FullName { get; set; }
        // Email is already in IdentityUser
        public string Department { get; set; }

        
        public ICollection<Ticket> Tickets { get; set; }
    }
}
