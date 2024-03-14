using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PracticeProject.Models;

namespace PracticeProject.Data
{
    public class ApiDbContext : IdentityDbContext
    {
        public ApiDbContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        {

        }

        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TodoItem>().HasOne(item => item.User).WithMany(item => item.TodoItems).HasForeignKey(item => item.UserId);
            builder.Entity<Comment>().HasOne(item => item.User).WithMany(item => item.Comments).HasForeignKey(item => item.UserId);
            base.OnModelCreating(builder);
            List<IdentityRole> roles = new List<IdentityRole> {
                new IdentityRole {
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                },
                new IdentityRole {
                    Name = "User",
                    NormalizedName = "USER",
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}