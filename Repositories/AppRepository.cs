using Microsoft.EntityFrameworkCore;
using OnlineTesting.Data;
using OnlineTesting.Dtos;
using OnlineTesting.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using OnlineTesting.Helpers;

namespace OnlineTesting.Repositories
{
    public class AppRepository : IAppRepository
    {
        private readonly DataContext _context;

        public AppRepository(DataContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0; //return true or false
        }

        public async Task<User> GetUser(int id, bool isCurrentUser)
        {
            var query = _context.Users.AsQueryable();

            if (isCurrentUser)
                query = query.IgnoreQueryFilters();

            var user = await query.FirstOrDefaultAsync(u => u.Id.Equals(id));

            return user;
        }

        public GeolocationDto GetUserIP()
        {
            var API_KEY = "at_ENcYw3BCj6JYzkowcbp4QfrhY0dpx";
            var API_URL = "https://geo.ipify.org/api/v1?";

            string url = API_URL + $"apiKey={API_KEY}";
            string resultData = string.Empty;

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

            using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                resultData = reader.ReadToEnd();
            }

            GeolocationDto model = JsonConvert.DeserializeObject<GeolocationDto>(resultData);

            return model;
        }

        public void SendEmail(string email, string subject, string body)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("Online Testing", "denis.kasarov@gmail.com"));

            message.To.Add(new MailboxAddress(subject, email));

            message.Subject = subject;

            string html = "<html><head><style type='text/css'> .body{background-color:#D3E3FC;}h1{margin:0;padding:0;}p{text-align: justify;font-size: 18px; } a.button{background-color: #D3E3FC;color: #333;text-decoration: none;padding: 5px 10px;border-radius: 4px;font-size: 18px; } a.button:hover{background-color: #d8d3fc;color: black; } @media only screen and (min-width: 1200px){div[class= content]{	width: 600px;} } @media only screen and (max-width: 1200px){div[class= content]{	width: 500px;} } @media only screen and (max-width: 992px){div[class= content]{	width: 400px;} } @media only screen and (max-width: 768px){div[class= content]{	width: 300px;} } @media only screen and (max-width: 576px){div[class= content]{	width: 90%;} } .content {border: 1px solid #00887A;border-radius: 4px;background-color: white;padding: 5px 15px;margin: 0 auto; } </style></head><body>" +
                "<div class='body'>" +
                "<div class='content'>" +
                "<h1>" + subject + "</h1>" +
                 body +
                "<br><br><p style='text-align: center; font-size: 12px; margin: 0;'>Best Regards, Online Testing System " + DateTime.Now.Year + "</p>" +
                "</div></div></body></html>";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = html;

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("denis.kasarov@gmail.com", "1224_kasarov");
                client.Send(message);
                client.Disconnect(true);
            }
        }

        public async Task<PagedList<QuestionsForExamDto>> GetQuestion(ExamDto examDto, int pageNumber)
        {
            var testQuestions = (from test in _context.Tests
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
                                 }).AsQueryable();

            return await PagedList<QuestionsForExamDto>.CreateAsync(testQuestions, pageNumber, 1);
        }
    }
}
