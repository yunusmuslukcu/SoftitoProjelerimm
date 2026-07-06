using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Abstract
{
    public interface IAppUserRepository : IRepository<AppUser>
    {
        Task<AppUser?> GetByUsernameAsync(string username);
    }
}
