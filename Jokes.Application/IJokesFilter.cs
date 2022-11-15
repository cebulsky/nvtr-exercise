namespace Jokes.Application;

public interface IJokesFilter
{
    IEnumerable<Joke> Filter(Joke[] jokesToFilter);
}