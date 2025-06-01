using Common.DAO;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Implementations
{
    public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UserDao> Users { get; set; } // Table des utilisateurs
        public DbSet<TransactionDao> Transactions { get; set; } // Table des utilisateurs

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configuration supplémentaire si nécessaire (ex: clés uniques, relations, etc.)
            modelBuilder.Entity<UserDao>()
                .HasIndex(u => u.Email)
                .IsUnique(); // Empêche les emails en double
        }
    }
}