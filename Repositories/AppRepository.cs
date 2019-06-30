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

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = body;

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("denis.kasarov@gmail.com", "1224_kasarov");
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
