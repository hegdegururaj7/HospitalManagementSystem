using MassTransit;
using Moq;

namespace PSL.MicroserviceTemplate.Domain.UnitTests.Common;

internal class ConsumeContextMock<TRequest, TResponse> : Mock<ConsumeContext<TRequest>>
where TRequest : class
where TResponse : class
{
    public TRequest Request { get; }
    public TResponse Response { get; private set; }

    public ConsumeContextMock(TRequest request)
    {
        Setup(x => x.Message).Returns(request);
        Setup(x => x.RespondAsync(It.IsAny<TResponse>()))
            .Callback<object>(response => Response = response as TResponse);
    }
}
