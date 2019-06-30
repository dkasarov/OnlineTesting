using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTesting.Models
{
    public class ExamHeader
    {
        public int StudentId { get; set; }

        public int TestId { get; set; }

        public ExamHeader(int studentId, int testId)
        {
            this.StudentId = studentId;
            this.TestId = testId;
        }
    }
}
