using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTesting.Entities
{
    [Table("TestQuestion")]
    public class TestQuestion
    {
        public int Id { get; set; }

        public string Question { get; set; }

        public int TestId { get; set; }

        public Test Test { get; set; }

        public int TestQuestionTypeId { get; set; }

        public TestQuestionType TestQuestionType { get; set; }

        public ICollection<TestQuestionAnswer> TestQuestionAnswers { get; set; }

        public ICollection<Exam> Exams { get; set; }
    }
}
