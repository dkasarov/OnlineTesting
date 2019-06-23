using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTesting.Entities
{
    [Table("ExamAnswer")]
    public class ExamAnswer
    {
        public int Id { get; set; }

        public ICollection<Exam> Exams { get; set; }

        public int TestQuestionAnswerId { get; set; }

        public TestQuestionAnswer TestQuestionAnswer { get; set; }
    }
}
