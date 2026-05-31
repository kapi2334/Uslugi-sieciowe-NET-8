using TravelQuotesApi.Models;
using Microsoft.EntityFrameworkCore;
namespace TravelQuotesApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Quote> Quotes { get; set; }
    }
}
