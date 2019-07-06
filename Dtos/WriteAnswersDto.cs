using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTesting.Dtos
{
    public class WriteAnswersDto
    {
        public int Id { get; set; } //TestQuestion.Id
        public List<int> Answers { get; set; }
    }
}
