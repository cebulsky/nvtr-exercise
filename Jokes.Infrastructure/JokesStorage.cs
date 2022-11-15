using Jokes.Application;

namespace Jokes.Infrastructure
{
    public class JokesStorage : IJokesStorage
    {
        private readonly JokesContext _context;

        public JokesStorage(JokesContext context)
        {
            _context = context;
        }

        public async Task SaveJokesAsync(IEnumerable<Joke> jokes)
        {
            foreach (var joke in jokes)
            {
                if (_context.Find<Joke>(joke.Id) == null)
                {
                    _context.Jokes.Add(joke);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
