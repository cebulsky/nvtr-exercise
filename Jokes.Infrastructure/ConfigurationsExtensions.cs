using Jokes.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Jokes.Infrastructure
{
    public static class ConfigurationsExtensions
    {

        public static IServiceCollection AddSqliteStorage(this IServiceCollection services, IConfiguration configuration)
        {

            ////services.AddOptions<DatabaseSettings>()
            //    .Configure<IConfiguration>((settings, configuration) =>
            //    {
            //        configuration.GetSection(nameof(DatabaseSettings)).Bind(settings);
            //    });

            var databaseSettings = new DatabaseSettings();
            configuration.GetSection(nameof(DatabaseSettings)).Bind(databaseSettings);

            var jokesProviderSettings = new JokesProviderSettings();
            configuration.GetSection(nameof(JokesProviderSettings)).Bind(jokesProviderSettings);

            services.AddHttpClient<IJokesProvider, JokesProvider>(client => client.BaseAddress = new Uri(jokesProviderSettings.EndpointUrl));

            return services
                .AddDbContext<JokesContext>(options => options.UseSqlite(databaseSettings.ConnectionString))
                .AddTransient<IJokesStorage, JokesStorage>();
        }
    }
}
