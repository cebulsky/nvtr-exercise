using FluentAssertions;
using RichardSzalay.MockHttp;

namespace Jokes.Infrastructure.Tests
{
    public class JokesProviderTests
    {
        private readonly MockHttpMessageHandler _httpHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly MockedRequest _request;

        public JokesProviderTests()
        {
            const string baseAddress = "https://example.com";
            const string resourceUrl = $"{baseAddress}/jokes/random";

            const string responseContentString = @"
{
    ""id"": ""id"",
    ""value"": ""Joke Content""
}
";

            _httpHandlerMock = new MockHttpMessageHandler();
            _request = _httpHandlerMock.When(HttpMethod.Get, resourceUrl)
                .Respond("application/json", responseContentString);

            _httpClient = _httpHandlerMock.ToHttpClient();
            _httpClient.BaseAddress = new Uri(baseAddress);
        }

        [Fact]
        public async Task ShouldGetDeclaredNumberOfJokes()
        {
            const int jokesToGetAmount = 2;

            var jokesProvider = new JokesProvider(_httpClient);

            var jokes = await jokesProvider.GetJokes(jokesToGetAmount);

            jokes.Length.Should().Be(jokesToGetAmount);
        }


        [Fact]
        public async Task ShouldCallExternalHttpApi()
        {
            const int jokesAmount = 5;

            var jokesProvider = new JokesProvider(_httpClient);

            await jokesProvider.GetJokes(jokesAmount);

            _httpHandlerMock.GetMatchCount(_request).Should().Be(jokesAmount);
        }
    }
}