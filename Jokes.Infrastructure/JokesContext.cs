using Jokes.Application;
using Microsoft.EntityFrameworkCore;

namespace Jokes.Infrastructure
{
    public class JokesContext : DbContext
    {
        public DbSet<Joke> Jokes { get; set; }

        public JokesContext(DbContextOptions<JokesContext> options) : base(options)
        {
            base.Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Joke>().HasKey(j => j.Id);
        }
    }
}