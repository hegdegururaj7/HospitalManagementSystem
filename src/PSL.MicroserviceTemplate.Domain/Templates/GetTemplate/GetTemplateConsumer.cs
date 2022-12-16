namespace PSL.MicroserviceTemplate.Domain.Templates.GetTemplate;

public class GetTemplateConsumer : IConsumer<GetTemplateRequest>
{
    private readonly ITemplateRepository _templateRepository;

    public GetTemplateConsumer(ITemplateRepository templateRepository)
    {
        _templateRepository = templateRepository;
    }

    public async Task Consume(ConsumeContext<GetTemplateRequest> context)
    {
        var template = await _templateRepository.GetTemplate(context.Message.TemplateId);
        await context.RespondAsync(new GetTemplateResponse(template));
    }
}
