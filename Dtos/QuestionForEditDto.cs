using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTesting.Dtos
{
    public class QuestionForEditDto
    {
        [Required]
        public string Question { get; set; }

        [Required]
        public int TestQuestionTypeId { get; set; }
    }
}
