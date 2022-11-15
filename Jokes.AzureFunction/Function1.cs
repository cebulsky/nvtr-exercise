using Jokes.Application;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Jokes.AzureFunction
{
    public class Function1
    {
        private readonly JokesPuller _jokesPuller;
        private readonly ILogger _logger;

        public Function1(ILoggerFactory loggerFactory, JokesPuller jokesPuller)
        {
            _jokesPuller = jokesPuller;
            _logger = loggerFactory.CreateLogger<Function1>();
        }

        [Function("PullJokesFunction")]
        public async Task Run([TimerTrigger("%CronExpression%", RunOnStartup = true)] MyInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            await _jokesPuller.PullJokes(1);
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
