using Jokes.Application.Model;

namespace Jokes.Application.Abstractions
{
    public interface IJokesStorage
    {
        Task SaveJokesAsync(IEnumerable<Joke> jokes);
    }
}