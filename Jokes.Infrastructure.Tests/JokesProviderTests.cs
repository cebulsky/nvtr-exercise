using System.Net;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using RichardSzalay.MockHttp;

namespace Jokes.Infrastructure.Tests
{
    public class JokesProviderTests
    {
        private readonly MockHttpMessageHandler _httpHandlerMock;
        private HttpClient _httpClient;
        private readonly MockedRequest _request;
        private JokesProvider _jokesProvider;
        private readonly Mock<ILogger<JokesProvider>> _logger;
        private readonly string _resourceUrl;

        public JokesProviderTests()
        {
            const string baseAddress = "https://example.com";
            _resourceUrl = $"{baseAddress}/jokes/random";

            const string responseContentString = @"
{
    ""id"": ""id"",
    ""value"": ""Joke Content""
}
";

            _httpHandlerMock = new MockHttpMessageHandler();
            _request = _httpHandlerMock.When(HttpMethod.Get, _resourceUrl)
                .Respond("application/json", responseContentString);

            _httpClient = _httpHandlerMock.ToHttpClient();
            _httpClient.BaseAddress = new Uri(baseAddress);

            _logger = new Mock<ILogger<JokesProvider>>();
            _jokesProvider = new JokesProvider(_httpClient, _logger.Object);
        }

        [Fact]
        public async Task ShouldGetDeclaredNumberOfJokes()
        {
            const int jokesToGetAmount = 2;

            var jokes = await _jokesProvider.GetJokesAsync(jokesToGetAmount);

            jokes.Length.Should().Be(jokesToGetAmount);
        }

        [Fact]
        public async Task ShouldCallExternalHttpApi()
        {
            const int jokesAmount = 5;

            await _jokesProvider.GetJokesAsync(jokesAmount);

            _httpHandlerMock.GetMatchCount(_request).Should().Be(jokesAmount);
        }

        [Fact]
        public async Task ShouldThrowJokesProviderExceptionWhenCannotProcessHttpResponse()
        {
            _httpHandlerMock.When(HttpMethod.Get, _resourceUrl)
                .Respond(HttpStatusCode.BadRequest);

            _httpClient = _httpHandlerMock.ToHttpClient();
            _jokesProvider = new JokesProvider(_httpClient, _logger.Object);

             await _jokesProvider.Invoking(async p => await p.GetJokesAsync(1)).Should().ThrowAsync<JokesProviderException>();
        }
    }
}