using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTesting.Dtos
{
    public class ExamDto
    {
        public int StudentId { get; set; }
        public int TestId { get; set; }
        public string Token { get; set; }
    }
}
