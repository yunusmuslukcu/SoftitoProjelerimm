using Data.Abstract;
using Model;
using SupermarketManagement.Data.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repository
{
    public class CategoryRepository : GenericRepository<Category>,ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }
    }
}
