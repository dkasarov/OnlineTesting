using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTesting.Dtos
{
    public class UserForUpdatePhotoDto
    {
        public IFormFile Photo { get; set; }
    }
}
