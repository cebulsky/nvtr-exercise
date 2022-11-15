using AutoMapper;
using Jokes.Application;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace Jokes.Infrastructure
{
    public class JokesStorage : IJokesStorage
    {
        private readonly JokesContext _context;
        private readonly ILogger<JokesStorage> _logger;
        private readonly IMapper _mapper;

        public JokesStorage(JokesContext context, ILogger<JokesStorage> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task SaveJokesAsync(IEnumerable<Joke> jokes)
        {
            try
            {
                foreach (var joke in jokes.Select(j => _mapper.Map<JokeDbEntity>(j)))
                {
                    if (JokeExists(joke))
                    {
                        continue;
                    }

                    _context.Jokes.Add(joke);
                }

                await _context.SaveChangesAsync();
            }
            catch (SqliteException sqliteException)
            {
                _logger.LogCritical(sqliteException, $"Database process error occurred in {nameof(JokesStorage)}");
                throw new JokesStorageException(sqliteException.Message, sqliteException);
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, "Unusual error thrown while saving jokes to storage.");
                throw;
            }
        }

        private bool JokeExists(JokeDbEntity joke)
        {
            var existingJoke = GetExistingJoke(joke);

            return existingJoke != null;
        }

        private JokeDbEntity? GetExistingJoke(JokeDbEntity joke)
        {
            return _context.Find<JokeDbEntity>(joke.Checksum);
        }
    }
}
