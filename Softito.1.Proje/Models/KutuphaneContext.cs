using Microsoft.EntityFrameworkCore;

namespace projectcodefrst.Models
{
    public class KutuphaneContext : DbContext
    {
        public KutuphaneContext(DbContextOptions<KutuphaneContext> dbcontext) : base(dbcontext)
        {

        }

        public DbSet<Kitap> Kitaps { get; set; }

        public DbSet<Yazar> Yazars { get; set; }

        public DbSet<Kategori> Kategoris { get; set; }

        public DbSet<Odunc> Oduncs { get; set; }
    }
}
