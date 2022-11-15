using FluentAssertions;
using Jokes.Application.Abstractions;
using Jokes.Application.Model;
using Moq;
using Tests.Common;

namespace Jokes.Application.Tests
{
    public class JokesPullerTests
    {
        private readonly Mock<IJokesProvider> _providerMock;
        private readonly Mock<IJokesStorage> _storageMock;
        private readonly Mock<IJokesFilter> _filterMock;
        private readonly JokesPuller _jokesPuller;

        public JokesPullerTests()
        {
            _providerMock = new Mock<IJokesProvider>();
            _storageMock = new Mock<IJokesStorage>();
            _filterMock = new Mock<IJokesFilter>();

            _jokesPuller = new JokesPuller(_providerMock.Object, _storageMock.Object, _filterMock.Object);
        }

        [Fact]
        public async Task ShouldCallProviderForJokes()
        {
            const int jokesToPull = 3;
            await _jokesPuller.PullJokes(jokesToPull);

            _providerMock.Verify(jp => jp.GetJokesAsync(jokesToPull));
        }

        [Fact]
        public async Task ShouldPassJokesFromProviderThroughFilter()
        {
            Joke[]? jokesPassedToFilter = null;
            _filterMock
                .Setup(s => s.Filter(It.IsAny<Joke[]>()))
                .Callback<Joke[]>(jokesArg => jokesPassedToFilter = jokesArg);

            var jokesFromProvider = JokesGenerator.GenerateMany(2);

            _providerMock.Setup(p => p.GetJokesAsync(It.IsAny<int>()))
                .ReturnsAsync(jokesFromProvider);

            await _jokesPuller.PullJokes(10);

            jokesPassedToFilter.Should().BeSameAs(jokesFromProvider);
        }

        [Fact]
        public async Task ShouldSaveOnlyFilteredJokesToStorage()
        {
            Joke[]? savedJokes = null;
            _storageMock
                .Setup(s => s.SaveJokesAsync(It.IsAny<IEnumerable<Joke>>()))
                .Callback<IEnumerable<Joke>>(jokes => savedJokes = jokes.ToArray());

            var jokes = JokesGenerator.GenerateMany(2);

            _filterMock.Setup(jp => jp.Filter(It.IsAny<Joke[]>()))
                .Returns(jokes);

            await _jokesPuller.PullJokes(jokes.Length);

            savedJokes.Should().BeEquivalentTo(jokes);
        }
    }
}
