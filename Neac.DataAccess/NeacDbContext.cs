using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.DataAccess
{
    public class NeacDbContext : DbContext
    {
        public NeacDbContext(DbContextOptions<NeacDbContext> options) : base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
    }
    public class NeacDbContextFactory : IDesignTimeDbContextFactory<NeacDbContext>
    {
        public NeacDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<NeacDbContext>();
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-V64DOTK;Initial Catalog=CoreDb;Integrated Security=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            return new NeacDbContext(optionsBuilder.Options);
        }
    }
}
