namespace Jokes.Application
{
    public interface IJokesStorage
    {
        Task SaveJokesAsync(IEnumerable<Joke> jokes);
    }
}