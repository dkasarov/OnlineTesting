using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTesting.Entities
{
    [Table("TestQuestionAnswer")]
    public class TestQuestionAnswer
    {
        public int Id { get; set; }

        public string Answer { get; set; }

        public bool IsCorrect { get; set; }

        public int TestQuestionId { get; set; }

        public TestQuestion TestQuestion { get; set; }

        public ICollection<ExamAnswer> ExamAnswers { get; set; }
    }
}
