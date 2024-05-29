using Microsoft.EntityFrameworkCore;
using TeraAuthApi.Domain.Entities;

namespace TeraAuthApi.Infrastructure.DataContext
{
    public class TeraDbContext : DbContext
    {
        public TeraDbContext(DbContextOptions<TeraDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = Guid.NewGuid(), Name = "User" },
                new Role { Id = Guid.NewGuid(), Name = "Admin" }
            );
            
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Role>()
                .HasIndex(r => r.Name)
                .IsUnique();
        }
    }
}