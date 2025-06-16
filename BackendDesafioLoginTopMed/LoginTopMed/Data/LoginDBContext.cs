using LoginTopMed.Models;
using Microsoft.EntityFrameworkCore;

namespace LoginTopMed.Data;

public class LoginDBContext : DbContext
{
    public LoginDBContext(DbContextOptions<LoginDBContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PasswordHash).IsRequired();

            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "user_1",
                    Email = "user1@topmed.com",
                    Name = "Maria",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("usertopmed123*"),
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                },
                new User
                {
                    Id = 2,
                    Username = "user_2",
                    Email = "user2@topmed.com",
                    Name = "João",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("usertopmed123*"),
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                }
            );
    }
}
