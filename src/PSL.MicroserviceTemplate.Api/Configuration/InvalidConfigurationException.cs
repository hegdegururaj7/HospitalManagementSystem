using System.Runtime.Serialization;

namespace PSL.MicroserviceTemplate.Api.Configuration;

[Serializable]
public class InvalidConfigurationException : Exception
{
    public InvalidConfigurationException(string exceptionMessage) : base(exceptionMessage) { }

    protected InvalidConfigurationException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}

