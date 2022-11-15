using System.Runtime.Serialization;

namespace Jokes.Infrastructure;

[Serializable]
public class JokesProviderException : Exception
{
    public JokesProviderException()
    {

    }

    public JokesProviderException(string message) : base(message)
    {

    }

    public JokesProviderException(string message, Exception innerException) : base(message, innerException)
    {

    }

    protected JokesProviderException(SerializationInfo info, StreamingContext context) : base(info, context)
    {

    }
}