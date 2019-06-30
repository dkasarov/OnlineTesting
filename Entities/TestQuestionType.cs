using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTesting.Entities
{
    [Table("TestQuestionType")]
    public class TestQuestionType
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int NumberOfAnswers { get; set; }

        public string Type { get; set; }

        public ICollection<TestQuestion> TestQuestions { get; set; }
    }
}
