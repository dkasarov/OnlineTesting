using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTesting.Entities
{
    [Table("Test")]
    public class Test
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime ActiveToDateTime { get; set; }

        public TimeSpan Time { get; set; }

        public DateTime DateChanged { get; set; }

        public int TryTimes { get; set; }

        public string Note { get; set; }

        public int Level { get; set; }

        public string ForCountry { get; set; }

        public string ForCity { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public ICollection<TestQuestion> TestQuestions { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}
