using Microsoft.EntityFrameworkCore;
using BaseBallVR.Models;

namespace BaseBallVR.DB
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            :base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Member> Members { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>()
                .HasKey(c => new { c.Id, c.UserId });
        }
    }
}