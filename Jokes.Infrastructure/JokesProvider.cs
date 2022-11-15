using System.Net.Http.Json;
using Jokes.Application;

namespace Jokes.Infrastructure
{
    public class JokesProvider : IJokesProvider
    {
        private readonly HttpClient _httpClient;

        public JokesProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Joke[]> GetJokesAsync(int amount)
        {
            var jokes = new List<Joke>();

            for (var i = 0; i < amount; i++)
            {
                var joke = await _httpClient.GetFromJsonAsync<Joke>("jokes/random");
                if (joke != null)
                {
                    jokes.Add(joke);
                }
            }

            return jokes.ToArray();
        }
    }
}