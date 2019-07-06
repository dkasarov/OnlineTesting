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
    [AllowAnonymous]
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
                Response.AddExam(studentForAdd.Id, studentToTest.TestId, Id);

                return Ok(studentToTest.Test.Time);
            }

            return NotFound();
        }

        [HttpGet("getMenu")]
        public async Task<IActionResult> GetMenu()
        {
            //Вземане на studentId и testId от header - exam
            ExamDto examDto = JsonConvert.DeserializeObject<ExamDto>(Request.Headers["Exam"]);

            if (examDto.TestId == 0 || examDto.StudentId == 0 || String.IsNullOrEmpty(examDto.Token))
                return NotFound();

            var student = await _context.Students
                .Include(stt => stt.StudentToTest)
                .FirstOrDefaultAsync(s => s.Id == examDto.StudentId
                && s.StudentToTest.TestId == examDto.TestId
                && s.StudentToTest.Id == examDto.Token);

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

        [HttpGet("getQuestion")]
        public async Task<IActionResult> GetQuestion([FromQuery]int pageNumber)
        {
            //Вземане на studentId и testId от header - exam
            ExamDto examDto = JsonConvert.DeserializeObject<ExamDto>(Request.Headers["Exam"]);

            if (examDto.TestId == 0 || examDto.StudentId == 0 || String.IsNullOrEmpty(examDto.Token))
                return NotFound();

            var student = await _context.Students
                .Include(stt => stt.StudentToTest)
                .FirstOrDefaultAsync(s => s.Id == examDto.StudentId
                && s.StudentToTest.TestId == examDto.TestId
                && s.StudentToTest.Id == examDto.Token);

            if (student == null)
                return Unauthorized();

            var question = await _repo.GetQuestion(examDto, pageNumber);

            if (question.Count() == 0)
                return NotFound();

            Response.AddPagination(question.CurrentPage, question.PageSize,
                question.TotalCount, question.TotalPages);

            return Ok(question);
        }

        [HttpPost("writeAnswer")]
        public async Task<IActionResult> WriteAnswer(WriteAnswersDto writeAnswersDto)
        {
            //Вземане на studentId и testId от header - exam
            ExamDto examDto = JsonConvert.DeserializeObject<ExamDto>(Request.Headers["Exam"]);

            if (examDto.TestId == 0 || examDto.StudentId == 0 || String.IsNullOrEmpty(examDto.Token))
                return NotFound();

            var student = await _context.Students
                .Include(stt => stt.StudentToTest)
                .FirstOrDefaultAsync(s => s.Id == examDto.StudentId
                && s.StudentToTest.TestId == examDto.TestId
                && s.StudentToTest.Id == examDto.Token);

            if (student == null)
                return Unauthorized();

            var question = await _context.TestQuestions
                .Include(tqa => tqa.TestQuestionAnswers)
                .Include(tqt => tqt.TestQuestionType)
                .FirstOrDefaultAsync(tq => tq.Id == writeAnswersDto.Id);

            int studentAnswers = writeAnswersDto.Answers.Count();

            //Проверява дали броят на отговорите не превишата с този на типът тест
            if (question.TestQuestionType.NumberOfAnswers < studentAnswers)
                return BadRequest("You cheat!");

            var examAnswersForStudent = await _context.Exams
                .Include(ea => ea.ExamAnswer)
                .Where(e => e.StudentId == examDto.StudentId &&
                    e.TestQuestionId == writeAnswersDto.Id)
                .ToListAsync();

            //Изтриване на старите отговори ако съществуват
            if (examAnswersForStudent.Count() != 0)
                _context.RemoveRange(examAnswersForStudent);

            //Entities за запис в базата
            ExamAnswer examAnswerToAdd = new ExamAnswer();
            Exam examToAdd = new Exam();

            foreach (var answer in writeAnswersDto.Answers)
            {
                //Мапване с въпрос
                var testQuestionAnswerForAdd = await _context.TestQuestionAnswers
                    .FirstOrDefaultAsync(q => q.Id == answer);
                examAnswerToAdd = new ExamAnswer
                {
                    TestQuestionAnswer = testQuestionAnswerForAdd
                };

                //Проверява дали ид на отговорите дадени от студентът отговарят на тези в базата за въпроса
                var questionsToCheck = question.TestQuestionAnswers.ToList();
                if (!questionsToCheck.Contains(testQuestionAnswerForAdd))
                    return BadRequest("You cheat");

                await _context.ExamAnswers.AddAsync(examAnswerToAdd);

                examToAdd = new Exam
                {
                    ExamAnswer = examAnswerToAdd,
                    Student = student,
                    TestQuestion = question
                };

                await _context.Exams.AddAsync(examToAdd);
            }

            if (await _context.SaveChangesAsync() > 0)
                return Ok();

            return BadRequest();
        }
    }
}