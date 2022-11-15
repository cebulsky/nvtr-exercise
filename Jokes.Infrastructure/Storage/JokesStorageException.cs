using System.Runtime.Serialization;

namespace Jokes.Infrastructure.Storage;

[Serializable]
public class JokesStorageException : Exception
{
    public JokesStorageException()
    {

    }

    public JokesStorageException(string message) : base(message)
    {

    }

    public JokesStorageException(string message, Exception innerException) : base(message, innerException)
    {

    }

    protected JokesStorageException(SerializationInfo info, StreamingContext context) : base(info, context)
    {

    }
}