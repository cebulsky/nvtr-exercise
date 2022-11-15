using Jokes.Application.Model;

namespace Jokes.Application.Abstractions
{
    public interface IJokesProvider
    {
        Task<Joke[]> GetJokesAsync(int jokesAmount);
    }
}