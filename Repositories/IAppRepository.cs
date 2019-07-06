using OnlineTesting.Dtos;
using OnlineTesting.Entities;
using OnlineTesting.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTesting.Repositories
{
    public interface IAppRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<User> GetUser(int id, bool isCurrentUser);
        GeolocationDto GetUserIP();

        void SendEmail(string toEmail, string subject, string body);

        Task<PagedList<QuestionsForExamDto>> GetQuestion(ExamDto examDto, int pageNumber);

        Task<bool> SaveAll();
    }
}
