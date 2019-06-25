using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineTesting.Data;
using OnlineTesting.Dtos;
using OnlineTesting.Helpers;
using OnlineTesting.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineTesting.Controllers
{
    //[ServiceFilter(typeof(LogUserActivity))]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IAppRepository _repo;
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public UsersController(IAppRepository repo,
            IMapper mapper,
            DataContext context)
        {
            _repo = repo;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var isCurrentUser = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) == id;

            var user = await _repo.GetUser(id, isCurrentUser);

            var userToReturn = _mapper.Map<UserForDetailsDto>(user);

            return Ok(userToReturn);
        }

        [HttpPut("updateUser/{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute]int id, UserForUpdateDto userForUpdateDto)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(id, true);

            _mapper.Map(userForUpdateDto, userFromRepo);

            _context.Users.Update(userFromRepo);

            if (await _context.SaveChangesAsync() > 0)
                return Ok();

            throw new Exception($"Updating user {id} failed on save");
        }

        [HttpPut("addPhotoForUser/{id}")]
        public async Task<IActionResult> AddPhotoForUser([FromRoute]int id, [FromForm]UserForUpdatePhotoDto userForUpdatePhotoDto)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(id, true);

            userFromRepo.Photo = userForUpdatePhotoDto.Photo.ConvertPhotoToArray();

            _context.Users.Update(userFromRepo);

            if (await _context.SaveChangesAsync() > 0)
                return Ok(userFromRepo.Photo);

            throw new Exception($"Updating user {id} failed on save");
        }
    }
}
