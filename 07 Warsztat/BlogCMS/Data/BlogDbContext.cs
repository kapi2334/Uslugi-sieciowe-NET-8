using Microsoft.EntityFrameworkCore;
using System.Collections;
using BlogCMS.Models;
namespace BlogCMS.Data
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options)
            : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<LoginModel> RegisteredUsers { get; set; }
    }
}
