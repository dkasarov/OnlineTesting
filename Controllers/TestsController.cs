using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineTesting.Data;
using OnlineTesting.Dtos;
using OnlineTesting.Entities;
using OnlineTesting.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineTesting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IAppRepository _repo;
        private readonly IMapper _mapper;

        public TestsController(DataContext context,
            UserManager<User> userManager,
            IAppRepository repo,
            IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _repo = repo;
            _mapper = mapper;
        }

        [HttpPost("addTest")]
        public async Task<IActionResult> AddTest(TestForAddingDto testForAddingDto)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == testForAddingDto.CategoryId);

            var testToAdd = _mapper.Map<Test>(testForAddingDto);

            testToAdd.User = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.Name).Value);
            testToAdd.Category = category;

            await _context.Tests.AddAsync(testToAdd);

            if (await _context.SaveChangesAsync() > 0)
                return Ok();

            return BadRequest();
        }
    }
}
