using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTesting.Dtos
{
    public class TestForAddingDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime ActiveToDateTime { get; set; }

        [Required]
        public TimeSpan Time { get; set; }

        [Required]
        public int TryTimes { get; set; }

        public string Note { get; set; }

        [Required]
        public int Level { get; set; }

        public string ForCountry { get; set; }

        public string ForCity { get; set; }

        public int CategoryId { get; set; }
    }
}
