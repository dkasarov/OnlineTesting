using OnlineTesting.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTesting.Repositories
{
    public interface IAppRepository
    {
        Task<User> GetUser(int id, bool isCurrentUser);
    }
}
