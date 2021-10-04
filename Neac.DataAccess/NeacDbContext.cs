using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
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
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            //var configuration = new ConfigurationBuilder()
            //    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
            //    .AddJsonFile("appsettings.json", optional: false)
            //    .AddJsonFile($"appsettings.{envName}.json", optional: false)
            //    .Build();

            var optionsBuilder = new DbContextOptionsBuilder<NeacDbContext>();
            //optionsBuilder.UseSqlServer(configuration.GetConnectionString("NeacDbContext"));
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-7JD9JER;Initial Catalog=CoreDb;Integrated Security=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            return new NeacDbContext(optionsBuilder.Options);
        }
    }
}
