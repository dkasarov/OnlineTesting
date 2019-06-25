using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTesting.Entities
{
    public class User : IdentityUser<int>
    {
        public string City { get; set; }

        public string Country { get; set; }

        public string Organization { get; set; }

        public DateTime DateBirth { get; set; }

        public string Gender { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastActive { get; set; }

        public byte[] Photo { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }

        public ICollection<Test> Tests { get; set; }

        public ICollection<Article> Articles { get; set; }
    }
}
