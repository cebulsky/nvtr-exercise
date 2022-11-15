using System.Net.Http.Json;
using Jokes.Application;
using Microsoft.Extensions.Logging;

namespace Jokes.Infrastructure
{
    public class JokesProvider : IJokesProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<JokesProvider> _logger;

        public JokesProvider(HttpClient httpClient, ILogger<JokesProvider> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<Joke[]> GetJokesAsync(int amount)
        {
            _logger.LogInformation($"Getting {amount} using {nameof(JokesProvider)} provider");

            var jokes = new List<Joke>();

            for (var i = 0; i < amount; i++)
            {
                var joke = await GetRandomJokeFromApi();
                if (joke != null)
                {
                    jokes.Add(joke);
                }
            }

            _logger.LogInformation($"Collected {jokes.Count} jokes using {nameof(JokesProvider)} provider");
            return jokes.ToArray();
        }

        private async Task<Joke?> GetRandomJokeFromApi()
        {
            try
            {
                var joke = await _httpClient.GetFromJsonAsync<Joke>("jokes/random");
                return joke;
            }
            catch (HttpRequestException requestException)
            {
                _logger.LogError(requestException, "Error occurred while getting random joke from external API");
                throw new JokesProviderException(requestException.Message, requestException);
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, "Unusual error thrown while getting random joke from external API");
                throw new JokesProviderException(exception.Message, exception);
            }
        }
    }
}