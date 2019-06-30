using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
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
using OnlineTesting.Helpers;
using OnlineTesting.Repositories;
using Newtonsoft.Json;

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
        [HttpPost("startExam/{Id}")]
        public async Task<IActionResult> StartExam(string Id, StudentForRegisterDto studentForRegisterDto)
        {
            var studentToTest = await _context.StudentToTests
                .Include(t => t.Test)
                .FirstOrDefaultAsync(s => s.Email == studentForRegisterDto.Email &&
                                     s.Id == Id);

            //Дали съществува тест за решаване
            if (studentToTest.Test == null)
                return NotFound();

            //Проверка дали има достъп до теста
            if (studentToTest == null)
                return BadRequest("You do not have permissions to make this test");

            //Дали вече го е решавал
            var checkStudent = await (from stud in _context.Students
                                      join exam in _context.Exams on stud.Id equals exam.StudentId
                                      join question in _context.TestQuestions on exam.TestQuestionId equals question.Id
                                      join tet in _context.Tests on question.TestId equals tet.Id
                                      join stt in _context.StudentToTests on tet.Id equals stt.TestId
                                      where stt.Id == Id
                                      select true).FirstOrDefaultAsync();

            if (checkStudent)
                return BadRequest("You have already done this test");

            //Вземане на данните за студентът от ipify.org
            var studentIp = _repo.GetUserIP();

            //Client PC IP
            string hostName = Dns.GetHostName(); // Retrive the Name of HOST
            //Get the IP
            string LocalIP = Dns.GetHostByName(hostName).AddressList[1].ToString();

            //Мапване на имената и данните от ipify.org
            var studentForAdd = _mapper.Map<Student>(studentIp);
            studentForAdd.LocalIP = LocalIP;
            studentForAdd.StudentToTest = studentToTest;

            //Запис
            await _context.Students.AddAsync(studentForAdd);

            if (await _context.SaveChangesAsync() > 0)
            {
                Response.AddExam(studentForAdd.Id, studentToTest.TestId);

                return Ok();
            }

            return NotFound();
        }

        [AllowAnonymous]
        [HttpGet("getMenu")]
        public async Task<IActionResult> GetMenu()
        {
            //Вземане на studentId и testId от header - exam
            ExamDto examDto = JsonConvert.DeserializeObject<ExamDto>(Request.Headers["Exam"]);

            if (examDto.TestId == 0 || examDto.StudentId == 0)
                return NotFound();

            var student = await _context.Students
                .Include(stt => stt.StudentToTest)
                .FirstOrDefaultAsync(s => s.Id == examDto.StudentId && s.StudentToTest.TestId == examDto.TestId);

            if (student == null)
                return Unauthorized();

            var testQuestionsMenu = await (from test in _context.Tests
                                           join tq in _context.TestQuestions on test.Id equals tq.TestId
                                           join stt in _context.StudentToTests on test.Id equals stt.TestId
                                           join s in _context.Students on stt.Id equals s.StudentToTestId
                                           where test.Id == examDto.TestId && s.Id == examDto.StudentId
                                           select new QuestionsMenuDto()
                                           {
                                               QuestionId = tq.Id
                                           }).ToListAsync();

            return Ok(new
            {
                Count = testQuestionsMenu.Count(),
                Questions = testQuestionsMenu
            });
        }

        [AllowAnonymous]
        [HttpGet("getQuestions")]
        public async Task<IActionResult> GetQuestions()
        {
            //Вземане на studentId и testId от header - exam
            ExamDto examDto = JsonConvert.DeserializeObject<ExamDto>(Request.Headers["Exam"]);

            if (examDto.TestId == 0 || examDto.StudentId == 0)
                return NotFound();

            var student = await _context.Students
                .Include(stt => stt.StudentToTest)
                .FirstOrDefaultAsync(s => s.Id == examDto.StudentId && s.StudentToTest.TestId == examDto.TestId);

            if (student == null)
                return Unauthorized();

            var testQuestions = await (from test in _context.Tests
                                       join tq in _context.TestQuestions on test.Id equals tq.TestId
                                       join tyq in _context.TestQuestionTypes on tq.TestQuestionTypeId equals tyq.Id
                                       join stt in _context.StudentToTests on test.Id equals stt.TestId
                                       join s in _context.Students on stt.Id equals s.StudentToTestId
                                       where test.Id == examDto.TestId && s.Id == examDto.StudentId
                                       select new QuestionsForExamDto()
                                       {
                                           Id = tq.Id,
                                           Question = tq.Question,
                                           Answers = (from a in _context.TestQuestionAnswers
                                                      where a.TestQuestionId == tq.Id
                                                      select new Answer
                                                      {
                                                          Id = a.Id,
                                                          AnswerText = a.Answer
                                                      }).ToList(),
                                           IsMultipleAnswer = tyq.Type == "radio" ? false : true
                                       }).ToListAsync();

            return Ok(testQuestions);
        }
    }
}