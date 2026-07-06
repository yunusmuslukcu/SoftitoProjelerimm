using Data.Abstract;
using Microsoft.EntityFrameworkCore;
using Model;
using SupermarketManagement.Data.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repository
{
    public class AppUserRepository : GenericRepository<AppUser>,IAppUserRepository
    {
        public AppUserRepository(AppDbContext context) : base(context)
        {
        }

        
        public async Task<AppUser?> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.Branch) 
                .FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
