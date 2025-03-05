using Microsoft.EntityFrameworkCore;
using Common.DAO;

namespace DataAccess
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbContext(DbContextOptions<DbContext> options) : base(options)
        {
        }

        public DbSet<UserDao> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDao>().HasIndex(u => u.Email).IsUnique();
        }
    }
}