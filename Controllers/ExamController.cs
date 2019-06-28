using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [Obsolete]
        [AllowAnonymous]
        [HttpPost("exam/{Id}")]
        public async Task<IActionResult> Exam(int Id, StudentForRegisterDto studNames)
        {
            //тест за решаване
            var test = await _context.Tests.FirstOrDefaultAsync(t => t.Id == Id);

            //Вземане на данните за студентът от ipify.org
            var studentIp = _repo.GetUserIP();

            //Client PC IP
            string hostName = Dns.GetHostName(); // Retrive the Name of HOST
            //Get the IP
            string LocalIP = Dns.GetHostByName(hostName).AddressList[1].ToString();

            //Мапване на имената и данните от ipify.org
            var studentForAdd = _mapper.Map<Student>(studentIp);
            studentForAdd.LocalIP = LocalIP;
            studentForAdd.FirstName = studNames.FirstName;
            studentForAdd.LastName = studNames.LastName;

            //Дали съществува
            if (test == null)
                return NotFound();

            //Проверки за ограниченията за ползване - country, city
            if (!String.IsNullOrEmpty(test.ForCountry) && test.ForCountry != studentForAdd.Country ||
                !String.IsNullOrEmpty(test.ForCity) && test.ForCity != studentForAdd.City)
                return BadRequest("You cannot do this test, bacause you are not in the right country or city");

            var checkStudent = await (from stud in _context.Students
                                      join exam in _context.Exams on stud.Id equals exam.StudentId
                                      join question in _context.TestQuestions on exam.TestQuestionId equals question.Id
                                      join tet in _context.Tests on question.TestId equals tet.Id
                                      where stud.NetworkIP == studentForAdd.NetworkIP && stud.LocalIP == studentForAdd.LocalIP && tet.Id == Id
                                      select true).FirstOrDefaultAsync();

            if (checkStudent)
                return BadRequest("You have already done this test");

            await _context.Students.AddAsync(studentForAdd);

            if (await _context.SaveChangesAsync() > 0)
                return Ok(new
                {
                    studentId = studentForAdd.Id,
                    testId = Id
                });

            return NotFound();
        }


    }
}