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
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace OnlineTesting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IAppRepository _repo;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public TestsController(DataContext context,
            IAppRepository repo,
            UserManager<User> userManager,
            IMapper mapper)
        {
            _context = context;
            _repo = repo;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpPost("addTest")]
        public async Task<IActionResult> AddTest(TestForAddingDto testForAddingDto)
        {
            //Мапване с категорията
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == testForAddingDto.CategoryId);

            //Test class
            var testToAdd = _mapper.Map<Test>(testForAddingDto);

            //Мапване с логнатият потребител
            testToAdd.User = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.Name).Value);
            testToAdd.Category = category;

            await _context.Tests.AddAsync(testToAdd);

            if (await _context.SaveChangesAsync() > 0)
            {
                //Връщане на нововъденият тест
                return Ok(new
                {
                    testId = testToAdd.Id
                });
            }

            return BadRequest();
        }

        [HttpPut("editTest/{testId}")]
        public async Task<IActionResult> EditTest(int testId, TestForAddingDto testForEditDto)
        {
            var test = await _context.Tests.FirstOrDefaultAsync(t => t.Id == testId);

            if (test == null)
                return NotFound();

            if (test.UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            test.Name = testForEditDto.Name;
            test.ActiveToDateTime = testForEditDto.ActiveToDateTime;
            test.Time = testForEditDto.Time;
            test.Note = testForEditDto.Note;
            test.Level = testForEditDto.Level;
            test.CategoryId = testForEditDto.CategoryId;
            test.DateChanged = DateTime.Now;

            if (await _context.SaveChangesAsync() > 0)
                return Ok(test);

            return BadRequest();
        }

        [HttpPost("addQuestion")]
        public async Task<IActionResult> AddQuestion(QuestionForAddingDto questionForAddingDto)
        {
            //Мапване с тест и тип въпрос
            var test = await _context.Tests.FirstOrDefaultAsync(t => t.Id == questionForAddingDto.TestId);
            var questionType = await _context.TestQuestionTypes.FirstOrDefaultAsync(t => t.Id == questionForAddingDto.TestQuestionTypeId);

            //Проверява за съществуването на теста и дали е създаден от логнатият потребител
            if (test == null || test.UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            //TestQuestion class
            var questionToAdd = _mapper.Map<TestQuestion>(questionForAddingDto);

            questionToAdd.Test = test;
            questionToAdd.TestQuestionType = questionType;

            await _context.TestQuestions.AddAsync(questionToAdd);

            if (await _context.SaveChangesAsync() > 0)
            {
                //Връщане на нововъденият въпрос
                return Ok(new
                {
                    questionId = questionToAdd.Id
                });
            }

            return BadRequest();
        }

        [HttpPut("editQuestion/{questionId}")]
        public async Task<IActionResult> EditQuestion(int questionId, QuestionForEditDto questionForEditDto)
        {
            //Мапване с тест и тип въпрос
            var question = await _context.TestQuestions
                .Include(t => t.Test)
                .FirstOrDefaultAsync(t => t.Id == questionId);
            var questionType = await _context.TestQuestionTypes.FirstOrDefaultAsync(t => t.Id == questionForEditDto.TestQuestionTypeId);


            if (question == null)
                return NotFound();

            if (question.Test.UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            question.TestQuestionType = questionType;
            question.Test = question.Test;

            if (await _context.SaveChangesAsync() > 0)
            {
                //Връщане на редактираният въпрос
                return Ok(new
                {
                    questionId = question.Id
                });
            }

            return BadRequest();
        }

        [HttpPost("addAnswer/{testQuestionId}")]
        public async Task<IActionResult> AddAnswer(int testQuestionId, List<AnswerForAddingDto> answerForAddingDtos)
        {
            //Мапване с въпрос
            var testQuestion = await _context.TestQuestions
                .Include(at => at.TestQuestionType)
                .Include(ta => ta.TestQuestionAnswers)
                .Include(t => t.Test)
                .ThenInclude(u => u.User)
                .FirstOrDefaultAsync(t => t.Id == testQuestionId);

            //Проверява за съществуването на въпроса и дали е създаден от логнатият потребител
            if (testQuestion == null || testQuestion.Test.User.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            //Проверява дали броят на верните отговори съвпада с този на типът тест
            int numOfCorrectAnswers = answerForAddingDtos.Count(x => x.IsCorrect == true);

            if (testQuestion.TestQuestionType.NumberOfAnswers != numOfCorrectAnswers)
                return BadRequest("The number of correct answers do not match with the number of chosen answer type");

            IEnumerable<TestQuestionAnswer> answersToAdd = _mapper.Map<List<TestQuestionAnswer>>(answerForAddingDtos);

            //Мапване с въпросът
            foreach (var answer in answersToAdd)
            {
                answer.TestQuestion = testQuestion;
            }

            //Изтриване на старите отговори ако съществуват
            if (testQuestion.TestQuestionAnswers != null)
                _context.RemoveRange(testQuestion.TestQuestionAnswers);

            await _context.TestQuestionAnswers.AddRangeAsync(answersToAdd);

            if (await _context.SaveChangesAsync() > 0)
                return Ok();

            return BadRequest();
        }

        [HttpGet("getTestsForUser/{userId}")]
        public async Task<IActionResult> GetTestsForUser(int? userId)
        {
            //Проверява логнатият потребител
            if (userId == null || userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var tests = await _context.Tests
                .Include(c => c.Category)
                .Where(t => t.UserId == userId)
                .ToListAsync();

            return Ok(_mapper.Map<List<TestDetailsForUserDto>>(tests));
        }

        [HttpGet("getQuestionsForUserTest/{testId}")]
        public async Task<IActionResult> GetQuestionsForUserTest(int testId)
        {
            //Проверява логнатият потребител
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var questions = await (from testQuestion in _context.TestQuestions
                                   join testQuestionType in _context.TestQuestionTypes
                                   on testQuestion.TestQuestionTypeId equals testQuestionType.Id
                                   join test in _context.Tests
                                   on testQuestion.TestId equals test.Id
                                   where testQuestion.Test.UserId == userId && testQuestion.TestId == testId
                                   select new
                                   {
                                       testQuestionId = testQuestion.Id,
                                       question = testQuestion.Question,
                                       AnswerDetails = (from testQuestionAnswer in _context.TestQuestionAnswers
                                                        where testQuestionAnswer.TestQuestionId == testQuestion.Id
                                                        select new
                                                        {
                                                            answer = testQuestionAnswer.Answer,
                                                            isCorrect = testQuestionAnswer.IsCorrect
                                                        }).ToList(),
                                       testQuestionTypeId = testQuestionType.Id
                                   }).ToListAsync();

            if (questions.Count() > 0)
                return Ok(questions);

            return Unauthorized();
        }

        [HttpPost("addStudentToTest/{testId}")]
        public async Task<IActionResult> AddStudentToTest(int testId, List<StudentToTestAddingDto> studentToTestAddingDto)
        {
            //Проверка за съществуването и личността на теста
            var test = await _context.Tests.FirstOrDefaultAsync(t => t.Id == testId);

            if (test == null)
                return NotFound();

            if (test.UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            //Логнат потребител
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.Name).Value);

            IEnumerable<StudentToTest> studentToTestForAdd = _mapper.Map<List<StudentToTest>>(studentToTestAddingDto);

            foreach (var stud in studentToTestForAdd)
            {
                stud.Test = test;
                stud.User = user;
            }

            await _context.StudentToTests.AddRangeAsync(studentToTestForAdd);

            if (await _context.SaveChangesAsync() > 0)
            {
                //Изпращане на съобщение
                var subject = "Test request";
                var callbackUrl = String.Empty;
                var body = String.Empty;

                foreach (var stud in studentToTestForAdd)
                {
                    callbackUrl = $"https://localhost:44376/exams/{ stud.Id }";

                    body = $"<p>Hello,<br>" +
                        $"You have been invited to make test in 'Online Testing System'. " +
                        $"Click in the button below to start your test.</p>" +
                     $"<a href='{ HtmlEncoder.Default.Encode(callbackUrl) }' class='button' style='text-align: center;'>Start Test Now</a>";
                    _repo.SendEmail(stud.Email, subject, body);
                }

                return Ok();
            }

            return BadRequest();
        }
    }
}
