using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTesting.Dtos
{
    public class TestDetailsForUserDto
    {
        public int Id { get; set; }

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

        public string CategoryName { get; set; }
    }
}
