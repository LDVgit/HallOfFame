using Microsoft.EntityFrameworkCore;

namespace HallOfFame.Models
{
    public class DataContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Skill> Skills { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite("Data Source=Database.db");

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Skill>()
                .HasOne(s => s.Person)
                .WithMany(p => p.Skills)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
