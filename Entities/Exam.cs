using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTesting.Entities
{
    [Table("Exam")]
    public class Exam
    {
        public int StudentId { get; set; }

        public Student Student { get; set; }

        public int TestQuestionId { get; set; }

        public TestQuestion TestQuestion { get; set; }

        public int ExamAnswerId { get; set; }

        public ExamAnswer ExamAnswer { get; set; }
    }
}
