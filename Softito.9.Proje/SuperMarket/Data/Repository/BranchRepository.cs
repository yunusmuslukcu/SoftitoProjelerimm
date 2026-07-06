using Data.Abstract;
using Model;
using SupermarketManagement.Data.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repository
{
    public class BranchRepository : GenericRepository<Branch> , IBranchRepository
    {
        public BranchRepository(AppDbContext context) : base(context)
        {
        }
    }
}
