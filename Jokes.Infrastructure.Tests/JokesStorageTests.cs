using FluentAssertions;
using Jokes.Application;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Jokes.Infrastructure.Tests
{
    public class JokesStorageTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly JokesContext _context;
        private readonly JokesStorage _jokesStorage;

        public JokesStorageTests()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<JokesContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new JokesContext(options);
            _jokesStorage = new JokesStorage(_context);
        }

        [Fact]
        public async Task ShouldSaveJokes()
        {
            var jokes = new[]
            {
                new Joke
                {
                    Id = Guid.NewGuid().ToString(),
                    Value = "Chuck Norris"
                },
                new Joke
                {
                    Id = Guid.NewGuid().ToString(),
                    Value = "Chuck Norris is the best"
                }
            };

            await _jokesStorage.SaveJokesAsync(jokes);

            _context.Jokes.ToArray().Should().BeEquivalentTo(jokes);
        }

        [Fact]
        public async Task ShouldNotSaveDuplicateJokes()
        {
            var jokes = new List<Joke>
            {
                new Joke
                {
                    Id = Guid.NewGuid().ToString(),
                    Value = "Chuck Norris"
                },
                new Joke
                {
                    Id = Guid.NewGuid().ToString(),
                    Value = "Chuck Norris is the best"
                }
            };

            var duplicatedJoke = new Joke
            {
                Id = jokes.First().Id
            };

            await _jokesStorage.SaveJokesAsync(jokes.Concat(new[] { duplicatedJoke }));

            _context.Jokes.ToArray().Should().BeEquivalentTo(jokes);
        }

        public void Dispose()
        {
            _connection.Dispose();
            _context.Dispose();
        }
    }
}