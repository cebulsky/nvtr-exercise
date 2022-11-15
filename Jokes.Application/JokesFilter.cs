namespace Jokes.Application
{
    public class JokesFilter
    {
        public JokesFilter()
        {
        }

        public static IEnumerable<Joke> Filter(Joke[] jokesToFilter)
        {
            return jokesToFilter.Where(j => j.Value.Length <= 200);
        }
    }

    public class JokesFilterFake : JokesFilter
    {
        private readonly Joke[] _jokesToReturn;

        public JokesFilterFake(Joke[] jokesToReturn)
        {
            _jokesToReturn = jokesToReturn;
        }

        internal new IEnumerable<Joke> Filter(Joke[] toFilter)
        {
            return _jokesToReturn;
        }
    }
}