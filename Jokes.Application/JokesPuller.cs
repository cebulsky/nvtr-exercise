namespace Jokes.Application
{
    public class JokesPuller
    {
        private readonly IJokesProvider _jokesProvider;
        private readonly IJokesStorage _storage;

        public JokesPuller(IJokesProvider jokesProvider, IJokesStorage storage)
        {
            _jokesProvider = jokesProvider;
            _storage = storage;
        }

        public async Task PullJokes(int jokesToPull)
        {
            var jokes = await _jokesProvider.GetJokes(jokesToPull);

            await _storage.SaveJokesAsync(JokesFilter.Filter(jokes));
        }
    }
}