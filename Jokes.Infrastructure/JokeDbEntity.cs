namespace Jokes.Infrastructure;

public class JokeDbEntity
{
    public string Checksum { get; set; }

    public string Quote { get; set; }

    public string ExternalResourceId { get; set; }
}