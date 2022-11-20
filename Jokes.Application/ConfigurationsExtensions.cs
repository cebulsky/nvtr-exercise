using Jokes.Application.Abstractions;
using Jokes.Application.Filter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Jokes.Application
{
    public static class ConfigurationsExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationSettings();

            services.AddJokesFilter();

            services.AddTransient<JokesPuller>();

            return services;
        }

        private static void AddApplicationSettings(this IServiceCollection services)
        {
            services.AddOptions<ApplicationSettings>()
                .Configure<IConfiguration>((settings, config) =>
                {
                    config.GetSection(nameof(ApplicationSettings)).Bind(settings);
                });
        }

        private static void AddJokesFilter(this IServiceCollection services)
        {
            services.AddOptions<JokesFilterSettings>()
                .Configure<IConfiguration>((settings, config) =>
                {
                    config.GetSection(nameof(JokesFilterSettings)).Bind(settings);
                });

            services.AddTransient<IJokesFilter, JokesFilter>();
        }
    }
}
