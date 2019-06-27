using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineTesting.Data;
using OnlineTesting.Dtos;
using OnlineTesting.Entities;
using OnlineTesting.Repositories;

namespace OnlineTesting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IAppRepository _repo;

        public ExamController(DataContext context,
            UserManager<User> userManager,
            IMapper mapper,
            IAppRepository repo)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _repo = repo;
        }

        [AllowAnonymous]
        [HttpPost("exam/{testId}")]
        public IActionResult Exam(int testId, StudentForRegisterDto studNames)
        {
            //Вземане на данните за студентът от ipify.org
            var studentIp = _repo.GetUserIP();

            Student studentForAdd = new Student();

            //Мапване на имената и данните от ipify.org
            studentForAdd.FirstName = studNames.FirstName;
            studentForAdd.LastName = studNames.LastName;
            studentForAdd.IP = studentIp.IP;
            studentForAdd.Country = studentIp.Location.Country;
            studentForAdd.City = studentIp.Location.City;
            studentForAdd.Region = studentIp.Location.Region;
            studentForAdd.PostalCode = studentIp.Location.PostalCode;
            studentForAdd.Lat = studentIp.Location.Lat;
            studentForAdd.Lng = studentIp.Location.Lng;

            //Вземане на теста - за проверка на TryTimes

            return Ok(studentForAdd);
        }


    }
}