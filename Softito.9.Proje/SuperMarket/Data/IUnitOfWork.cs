using Data.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Categories { get; }
        IProductRepository Products { get; }
        IBranchRepository Branches { get; }
        ISupplierRepository Suppliers { get; }
        IAppUserRepository Users { get; }

  
        Task<int> SaveAsync();
    }
}
