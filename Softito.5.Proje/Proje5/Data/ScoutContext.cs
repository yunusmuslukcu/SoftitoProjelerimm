using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Proje5.Models;
using System.Data.Common;

namespace Proje5.Data
{
    public class ScoutContext : IdentityDbContext<IdentityUser>
    {
        public ScoutContext(DbContextOptions<ScoutContext> options) : base(options)
        {

        }

        public DbSet<Player> Players { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Position> Positions { get; set; }
    }
}
