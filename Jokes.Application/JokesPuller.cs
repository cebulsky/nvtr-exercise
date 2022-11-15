using Jokes.Application.Abstractions;

namespace Jokes.Application
{
    public class JokesPuller
    {
        private readonly IJokesProvider _jokesProvider;
        private readonly IJokesStorage _storage;
        private readonly IJokesFilter _jokesFilter;

        public JokesPuller(IJokesProvider jokesProvider, IJokesStorage storage, IJokesFilter jokesFilter)
        {
            _jokesProvider = jokesProvider;
            _storage = storage;
            _jokesFilter = jokesFilter;
        }

        public async Task PullJokes(int jokesToPull)
        {
            var jokes = await _jokesProvider.GetJokesAsync(jokesToPull);

            var filteredJokes = _jokesFilter.Filter(jokes);

            await _storage.SaveJokesAsync(filteredJokes);
        }
    }
}