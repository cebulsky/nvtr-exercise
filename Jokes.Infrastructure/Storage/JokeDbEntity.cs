namespace Jokes.Infrastructure.Storage;

public class JokeDbEntity
{
    public string Checksum { get; set; }

    public string Quote { get; set; }

    public string ExternalResourceId { get; set; }
}