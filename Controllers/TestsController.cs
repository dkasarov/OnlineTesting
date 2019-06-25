using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineTesting.Data;
using OnlineTesting.Entities;
using OnlineTesting.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTesting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ITestRepository _testRepository;
        private readonly IMapper _mapper;

        public TestsController(DataContext context,
            UserManager<User> userManager,
            ITestRepository testRepository,
            IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _testRepository = testRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddTest()
    }
}
