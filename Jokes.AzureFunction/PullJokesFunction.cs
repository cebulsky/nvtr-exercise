using Jokes.Application;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jokes.AzureFunction
{
    public class PullJokesFunction
    {
        private readonly JokesPuller _jokesPuller;
        private readonly ApplicationSettings _settings;
        private readonly ILogger _logger;

        public PullJokesFunction(ILoggerFactory loggerFactory, JokesPuller jokesPuller, IOptions<ApplicationSettings> settings)
        {
            _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
            _jokesPuller = jokesPuller;
            _logger = loggerFactory.CreateLogger<PullJokesFunction>();
        }

        [Function("PullJokesFunction")]
        public async Task Run([TimerTrigger("%CronExpression%", RunOnStartup = true)] MyInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            _logger.LogInformation($"Pulling {_settings.JokesToPullAmount} jokes");
            await _jokesPuller.PullJokes(_settings.JokesToPullAmount);
        }
    }

    public class MyInfo
    {
        public MyScheduleStatus ScheduleStatus { get; set; }

        public bool IsPastDue { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
