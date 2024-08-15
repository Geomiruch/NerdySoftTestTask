using Microsoft.EntityFrameworkCore;
using NerdySoftTestTask.Models;

namespace NerdySoftTestTask.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Announcement> Announcements { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}
