using Moq;
using PSL.MicroserviceTemplate.Domain.Primitives;
using PSL.MicroserviceTemplate.Domain.Templates;
using PSL.MicroserviceTemplate.Domain.Templates.GetTemplate;
using PSL.MicroserviceTemplate.Domain.UnitTests.Common;

namespace PSL.MicroserviceTemplate.Domain.UnitTests.Templates;

internal class GetTemplateConsumerTestWrapper
{
    // Default Test Properties
    public TemplateId KnownTemplateId { get; } = "166225af-9513-4894-9452-fdd9b4b7e7ab";
    public TemplateModel KnownTemplateModel { get; }

    // Mocks
    public Mock<ITemplateRepository> TemplateRepository { get; set; } = new();

    // Setup Mocks
    public GetTemplateConsumerTestWrapper()
    {
        KnownTemplateModel = new TemplateModel(KnownTemplateId, KnownTemplateId);

        TemplateRepository.Setup(r => r.GetTemplate(KnownTemplateId))
            .ReturnsAsync(KnownTemplateModel);
    }

    
    public GetTemplateConsumer CreateSubject()
    {
        return new GetTemplateConsumer(TemplateRepository.Object);
    }

    public ConsumeContextMock<GetTemplateRequest, GetTemplateResponse> CreateContextMock(GetTemplateRequest request)
    {
        return new ConsumeContextMock<GetTemplateRequest, GetTemplateResponse>(request);
    }
}
