using PSL.MicroserviceTemplate.Domain.Templates.GetTemplate;

namespace PSL.MicroserviceTemplate.Domain.UnitTests.Templates;

public class GetTemplateConsumerTests
{
    [Fact]
    public async Task Consume_ReturnsCorrectTemplate_WhenTemplateIdParameterIsValid()
    {
        // Arrange
        var wrapper = new GetTemplateConsumerTestWrapper();
        var sut = wrapper.CreateSubject();
        var request = new GetTemplateRequest(wrapper.KnownTemplateId);
        var context = wrapper.CreateContextMock(request);

        // Act
        await sut.Consume(context.Object);

        // Assert
        context.Response.Template.Should().NotBeNull();
        context.Response.Template.Id.Should().Be(wrapper.KnownTemplateId);
        context.Response.Template.Name.Should().Contain(wrapper.KnownTemplateId.ToString());
    }
}