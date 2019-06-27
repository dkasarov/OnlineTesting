using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTesting.Dtos
{
    public class AnswerForAddingDto
    {
        [Required]
        public string Answer { get; set; }

        [Required]
        public bool IsCorrect { get; set; }
    }
}
