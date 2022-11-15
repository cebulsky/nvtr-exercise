using Jokes.Application;

namespace Tests.Common
{
    public static class JokesGenerator
    {
        public static Joke GenerateJokeOfLength(int jokesLength)
        {
            var guid = Guid.NewGuid().ToString();
            return new Joke
            {
                Id = guid,
                Value = new string('a', jokesLength)
            };
        }

        public static Joke[] GenerateMany(int numberOfJokes)
        {
            var jokes = new List<Joke>();
            for (var i = 0; i < numberOfJokes; i++)
            {
                jokes.Add(GenerateJoke());
            }

            return jokes.ToArray();
        }

        private static Joke GenerateJoke()
        {
            var guid = Guid.NewGuid().ToString();
            return new Joke
            {
                Id = guid,
                Value = $"Joke {guid}"
            };
        }
    }
}