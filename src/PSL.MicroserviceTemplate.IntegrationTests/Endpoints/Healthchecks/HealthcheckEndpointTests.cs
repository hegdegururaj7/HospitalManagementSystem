using Microsoft.Extensions.Diagnostics.HealthChecks;
using PSL.MicroserviceTemplate.IntegrationTests.Helpers;
using System.Net;

namespace PSL.MicroserviceTemplate.IntegrationTests.Endpoints.Healthchecks;

[Collection(MicroserviceTemplateApiFactory.UsesWebApplicationFactory)]
public class HealthcheckEndpointTests
{
    [Fact]
    public async Task GivenGetRequestToHealth_ShouldReturnHealthyResponse()
    {
        // Arrange
        var waf = new MicroserviceTemplateApiFactory();
        var client = waf.CreateDefaultClient();

        Thread.Sleep(5000);

        // Act
        var response = await client.GetAsync("/health");

        var checkLoop = 0;
        while (response.StatusCode == HttpStatusCode.ServiceUnavailable && checkLoop < 5)
        {
            Thread.Sleep(100);
            response = await client.GetAsync("/health");
            checkLoop++;
        }

        // Assert
        response.Content.Deserialize<TestHealthReport>().Status.Should().Be(HealthStatus.Healthy);
    }

    public class TestHealthReport
    {
        public IReadOnlyDictionary<string, HealthReportEntry> Entries { get; set; }
        public HealthStatus Status { get; set; }
        public TimeSpan TotalDuration { get; set; }
    }
}
