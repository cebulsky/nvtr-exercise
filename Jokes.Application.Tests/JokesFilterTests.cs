using FluentAssertions;
using Microsoft.Extensions.Options;

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
        public void ShouldFilterOutJokesLongerThan200Characters()
        {
            var jokesToFilter = new[]
            {
                new Joke
                {
                    Id = "1",
                    Value = "1"
                },

                new Joke
                {
                    Id = "2",
                    Value = new string('a', 199)
                },

                new Joke
                {
                    Id = "2",
                    Value = new string('a', 200)
                },

                new Joke
                {
                    Id = "2",
                    Value = new string('a', 201)
                }
            };

            var filteredJokes = _jokesFilter.Filter(jokesToFilter);

            filteredJokes.Should().BeEquivalentTo(filteredJokes.Where(j => j.Value.Length <= _options.Value.MaxJokeLength));
        }
    }
}
