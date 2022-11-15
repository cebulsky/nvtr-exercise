using FluentAssertions;
using Jokes.Application.Filter;
using Microsoft.Extensions.Options;
using Tests.Common;

namespace Jokes.Application.Tests
{
    public class JokesFilterTests
    {
        private readonly IOptions<JokesFilterSettings> _options;
        private readonly JokesFilter _jokesFilter;


        public JokesFilterTests()
        {

           _options = Options.Create(new JokesFilterSettings
            {
                MaxJokeLength = 10
            });

            _jokesFilter = new JokesFilter(_options);
        }

        [Fact]
        public void ShouldFilterOutJokesLongerThanMaxLength()
        {
            var jokesToFilter = JokesGenerator.GenerateMany(3)
                .Concat(new []{JokesGenerator.GenerateJokeOfLength(_options.Value.MaxJokeLength + 1)})
                .ToArray();

            var filteredJokes = _jokesFilter.Filter(jokesToFilter);

            filteredJokes.Should().BeEquivalentTo(filteredJokes.Where(j => j.Value.Length <= _options.Value.MaxJokeLength));
        }
    }
}
