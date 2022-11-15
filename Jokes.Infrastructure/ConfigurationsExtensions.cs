using Jokes.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Jokes.Infrastructure
{
    public static class ConfigurationsExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddSqliteStorage(configuration)
                .AddJokesProvider(configuration)
                .AddAutoMapper(typeof(JokeMappingProfile));

            return services;
        }

        private static IServiceCollection AddSqliteStorage(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseSettings = new DatabaseSettings();
            configuration.GetSection(nameof(DatabaseSettings)).Bind(databaseSettings);

            services.AddJokesProvider(configuration);

            return services
                .AddDbContext<JokesContext>(options => options.UseSqlite(databaseSettings.ConnectionString))
                .AddTransient<IJokesStorage, JokesStorage>();
        }

        private static IServiceCollection AddJokesProvider(this IServiceCollection services, IConfiguration configuration)
        {
            var jokesProviderSettings = new JokesProviderSettings();
            configuration.GetSection(nameof(JokesProviderSettings)).Bind(jokesProviderSettings);

            services.AddHttpClient<IJokesProvider, JokesProvider>(client =>
                client.BaseAddress = new Uri(jokesProviderSettings.EndpointUrl));

            return services;
        }
    }
}
