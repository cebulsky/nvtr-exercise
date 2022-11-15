using Jokes.Application.Abstractions;
using Jokes.Application.Model;
using Microsoft.Extensions.Options;

namespace Jokes.Application.Filter
{
    public class JokesFilter : IJokesFilter
    {
        private readonly JokesFilterSettings _filterSettings;

        public JokesFilter(IOptions<JokesFilterSettings> options)
        {
            _filterSettings = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public IEnumerable<Joke> Filter(Joke[] jokesToFilter)
        {
            return jokesToFilter.Where(j => j.Value.Length <= _filterSettings.MaxJokeLength);
        }
    }
}