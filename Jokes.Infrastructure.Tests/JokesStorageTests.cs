using FluentAssertions;
using Jokes.Application;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Tests.Common;

namespace Jokes.Infrastructure.Tests
{
    public class JokesStorageTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly JokesContext _context;
        private readonly JokesStorage _jokesStorage;
        private readonly Mock<ILogger<JokesStorage>> _logger;

        public JokesStorageTests()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<JokesContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new JokesContext(options);
            _logger = new Mock<ILogger<JokesStorage>>();
            _jokesStorage = new JokesStorage(_context, _logger.Object);
        }

        [Fact]
        public async Task ShouldSaveJokes()
        {
            var jokes = JokesGenerator.GenerateMany(2);

            await _jokesStorage.SaveJokesAsync(jokes);

            _context.Jokes.ToArray().Should().BeEquivalentTo(jokes);
        }

        [Fact]
        public async Task ShouldNotSaveDuplicateJokes()
        {
            var jokes = JokesGenerator.GenerateMany(2);

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