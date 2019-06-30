using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTesting.Entities
{
    public class StudentToTest
    {
        [Key]
        public string Id { get; set; }

        public string Email { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public DateTime DateAdded { get; set; }

        public int TestId { get; set; }

        public Test Test { get; set; }

        public StudentToTest()
        {
            Id = Guid.NewGuid().ToString();
            DateAdded = DateTime.Now;
        }
    }
}
