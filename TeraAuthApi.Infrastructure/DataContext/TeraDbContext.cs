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
            var adminRoleId = Guid.NewGuid();
            var userRoleId = Guid.NewGuid();

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = userRoleId, Name = "User" },
                new Role { Id = adminRoleId, Name = "Admin" }
            );

            var adminUserId = Guid.NewGuid();
            var passwordHash = BCrypt.Net.BCrypt.HashPassword("admin");

            modelBuilder.Entity<User>().HasData(
                new User 
                { 
                    Id = adminUserId, 
                    Username = "admin", 
                    Email = "admin@domain.com", 
                    PasswordHash = passwordHash, 
                    CreatedAt = DateTime.UtcNow 
                }
            );

            modelBuilder.Entity<UserRole>().HasData(
                new UserRole 
                { 
                    Id = Guid.NewGuid(), 
                    UserId = adminUserId, 
                    RoleId = adminRoleId 
                }
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