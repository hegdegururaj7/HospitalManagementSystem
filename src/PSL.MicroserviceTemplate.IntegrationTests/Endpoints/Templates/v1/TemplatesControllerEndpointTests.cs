using PSL.MicroserviceTemplate.IntegrationTests.Helpers;
using System.Net;

namespace PSL.MicroserviceTemplate.IntegrationTests.Endpoints.Templates.v1;

[Collection(MicroserviceTemplateApiFactory.UsesWebApplicationFactory)]
public class TemplatesControllerEndpointTests
{
    [Fact]
    public async Task Get_WithTemplateId_ReturnsKnownTemplate_WhenKnownTemplateIdPassed()
    {
        // Arrange
        using var apiFactory = new MicroserviceTemplateApiFactory();
        using var client = apiFactory.CreateClientWithAuthenticatedUser();

        // Act
        using var response = await client.GetAsync("api/v1/templates/08739b0f-0da4-4ff0-a0e1-dfe3669aa6d7");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}