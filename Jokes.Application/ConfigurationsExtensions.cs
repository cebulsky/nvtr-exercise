using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Jokes.Application
{
    public static class ConfigurationsExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddJokesFilter();

            services.AddTransient<JokesPuller>();

            return services;
        }

        private static void AddJokesFilter(this IServiceCollection services)
        {
            services.AddOptions<JokesFilter>()
                .Configure<IConfiguration>((settings, config) =>
                {
                    config.GetSection(nameof(JokesFilterSettings)).Bind(settings);
                });

            services.AddTransient<IJokesFilter, JokesFilter>();
        }
    }
}
