using Jokes.Application.Model;

namespace Jokes.Application.Abstractions;

public interface IJokesFilter
{
    IEnumerable<Joke> Filter(Joke[] jokesToFilter);
}