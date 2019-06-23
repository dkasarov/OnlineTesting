using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OnlineTesting.Data;
using OnlineTesting.Dtos;
using OnlineTesting.Entities;
using OnlineTesting.Repositories;

namespace OnlineTesting.Controllers
{
    [Authorize(Policy = "AdminRole")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IAppRepository _repo;
        private readonly IMapper _mapper;

        public AdminController(DataContext context,
            UserManager<User> userManager,
            IAppRepository repo,
            IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet("usersWithRoles")]
        public async Task<IActionResult> GetUsersWithRoles()
        {
            var userList = await (from user in _context.Users
                                  orderby user.UserName
                                  select new
                                  {
                                      Id = user.Id,
                                      UserName = user.UserName,
                                      Roles = (from userRole in user.UserRoles
                                               join role in _context.Roles
                                               on userRole.RoleId equals role.Id
                                               select role.Name).ToList()
                                  }).ToListAsync();

            return Ok(userList);
        }

        [HttpPost("editRoles/{userName}")]
        public async Task<IActionResult> EditRoles(string userName, RoleEditDto roleEditDto)
        {
            var user = await _userManager.FindByNameAsync(userName);

            var userRoles = await _userManager.GetRolesAsync(user);

            var selectedRoles = roleEditDto.RoleNames;

            selectedRoles = selectedRoles ?? new string[] { };

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded)
                return BadRequest("Failed to add to roles");

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded)
                return BadRequest("Failed to remove the roles");

            return Ok(await _userManager.GetRolesAsync(user));
        }

        [HttpGet("getCategories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();

            return Ok(categories);
        }

        [HttpPost("addCategory")]
        public async Task<IActionResult> AddCategory(CategoryForCreationDto categoryForCreationDto)
        {
            var categoryToAdd = _mapper.Map<Category>(categoryForCreationDto);

            await _context.Categories.AddAsync(categoryToAdd);

            if (await _context.SaveChangesAsync() > 0)
                return Ok();

            return BadRequest();
        }

        [HttpPut("editCategory/{categoryId}")]
        public async Task<IActionResult> EditCategory(int categoryId, CategoryForEditDto categoryForEditDto)
        {
            if (categoryId != categoryForEditDto.Id)
                return BadRequest("Category was not found");

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryForEditDto.Id);

            category.Name = categoryForEditDto.Name;

            if (await _context.SaveChangesAsync() > 0)
                return Ok();

            return BadRequest();
        }

        [HttpDelete("deleteCategory/{categoryId}")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            if(category == null)
                return BadRequest("Category was not found");

            _context.Categories.Remove(category);

            if (await _context.SaveChangesAsync() > 0)
                return Ok();

            return BadRequest();
        }
    }
}
