using OnlineTesting.Dtos;
using OnlineTesting.Entities;
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
        Task<bool> SaveAll();
    }
}
