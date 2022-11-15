namespace Jokes.Application
{
    public interface IJokesProvider
    {
        Task<Joke[]> GetJokesAsync(int jokesAmount);
    }
}