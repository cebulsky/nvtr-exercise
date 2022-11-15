using System.Net.Http.Json;
using Jokes.Application.Abstractions;
using Jokes.Application.Model;
using Microsoft.Extensions.Logging;

namespace Jokes.Infrastructure.JokesApi
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

            var jokes = await CollectJokesFromTasks(amount);

            _logger.LogInformation($"Collected {jokes.Length} jokes using {nameof(JokesProvider)} provider");
            return jokes;
        }

        private async Task<Joke[]> CollectJokesFromTasks(int numberOfJokes)
        {
            var tasks = PrepareGettingJokeTasks(numberOfJokes);

            await Task.WhenAll(tasks);

            return tasks.
                Where(t => t.Result != null).
                Select(t => t.Result).ToArray()!;
        }

        private List<Task<Joke?>> PrepareGettingJokeTasks(int amount)
        {
            var tasks = new List<Task<Joke?>>();

            for (var i = 0; i < amount; i++)
            {
                tasks.Add(GetRandomJokeFromApi());
            }

            return tasks;
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
                throw;
            }
        }
    }
}