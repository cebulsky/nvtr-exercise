using Microsoft.Extensions.Options;

namespace Jokes.Application
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


    // remove
    //public class JokesFilterFake : JokesFilter
    //{
    //    private readonly Joke[] _jokesToReturn;

    //    public JokesFilterFake(Joke[] jokesToReturn)
    //    {
    //        _jokesToReturn = jokesToReturn;
    //    }

    //    internal new IEnumerable<Joke> Filter(Joke[] toFilter)
    //    {
    //        return _jokesToReturn;
    //    }
    //}
}