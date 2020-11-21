using Microsoft.EntityFrameworkCore;

namespace HallOfFame.Models
{
    public class DataContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Skill> Skills { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Skill>()
                .HasOne(s => s.Person)
                .WithMany(p => p.Skills)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
