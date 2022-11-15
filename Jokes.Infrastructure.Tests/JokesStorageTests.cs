using AutoMapper;
using FluentAssertions;
using Jokes.Application.Model;
using Jokes.Infrastructure.Storage;
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
        private readonly IMapper _mapper;

        public JokesStorageTests()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<JokesContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new JokesContext(options);
            _logger = new Mock<ILogger<JokesStorage>>();

            var mapperConfiguration = new MapperConfiguration(cfg =>
                cfg.AddProfile(typeof(JokeMappingProfile)));

            _mapper = mapperConfiguration.CreateMapper();

            _jokesStorage = new JokesStorage(_context, _logger.Object, _mapper);
        }

        [Fact]
        public async Task ShouldSaveJokes()
        {
            var jokes = JokesGenerator.GenerateMany(2);

            await _jokesStorage.SaveJokesAsync(jokes);

            var dbJokes = _mapper.Map<JokeDbEntity[]>(jokes);

            _context.Jokes.ToArray().Should().BeEquivalentTo(dbJokes);
        }

        [Fact]
        public async Task ShouldNotSaveDuplicateJokes()
        {
            var jokes = JokesGenerator.GenerateMany(2);

            var duplicatedJoke = new Joke
            {
                Id = Guid.NewGuid().ToString(),
                Value = jokes.First().Value
            };

            await _jokesStorage.SaveJokesAsync(jokes.Concat(new[] { duplicatedJoke }));

            var dbJokes = _mapper.Map<JokeDbEntity[]>(jokes);

            _context.Jokes.ToArray().Should().BeEquivalentTo(dbJokes);
        }

        public void Dispose()
        {
            _connection.Dispose();
            _context.Dispose();
        }
    }
}