using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTesting.Dtos
{
    public class QuestionsForExamDto
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public List<Answer> Answers { get; set; }
        public bool IsMultipleAnswer { get; set; }
    }

    public class Answer
    {
        public int Id { get; set; }
        public string AnswerText { get; set; }

    }
}
