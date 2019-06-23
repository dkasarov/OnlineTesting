using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTesting.Dtos
{
    public class CategoryForCreationDto
    {
        [Required]
        public string Name { get; set; }
    }
}
