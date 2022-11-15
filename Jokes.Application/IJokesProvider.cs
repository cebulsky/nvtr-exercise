namespace Jokes.Application
{
    public interface IJokesProvider
    {
        Task<Joke[]> GetJokes(int jokesAmount);
    }
}