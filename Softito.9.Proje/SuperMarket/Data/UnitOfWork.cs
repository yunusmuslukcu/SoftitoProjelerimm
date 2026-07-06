using Data;
using Data.Abstract;
using Data.Repository;
using SupermarketManagement.Data.Repository;

namespace SupermarketManagement.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        private ICategoryRepository? _categoryRepository;
        private IProductRepository? _productRepository;
        private IBranchRepository? _branchRepository;
        private ISupplierRepository? _supplierRepository;
        private IAppUserRepository? _userRepository;

        public UnitOfWork(AppDbContext context)
        {

            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public ICategoryRepository Categories => _categoryRepository ??= new CategoryRepository(_context);
        public IProductRepository Products => _productRepository ??= new ProductRepository(_context);
        public IBranchRepository Branches => _branchRepository ??= new BranchRepository(_context);
        public ISupplierRepository Suppliers => _supplierRepository ??= new SupplierRepository(_context);
        public IAppUserRepository Users => _userRepository ??= new AppUserRepository(_context);

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}