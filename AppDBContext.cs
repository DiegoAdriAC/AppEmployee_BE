using AppEmployee.Models;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employee { get; set; }

        public DbSet<User> User { get; set; }
    }
}
