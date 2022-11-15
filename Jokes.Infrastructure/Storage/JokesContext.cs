using Jokes.Application;
using Microsoft.EntityFrameworkCore;

namespace Jokes.Infrastructure.Storage
{
    public class JokesContext : DbContext
    {
        public DbSet<JokeDbEntity> Jokes { get; set; }

        public JokesContext(DbContextOptions<JokesContext> options) : base(options)
        {
            base.Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<JokeDbEntity>().HasKey(j => j.Checksum);
        }
    }
}