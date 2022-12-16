using PSL.MicroserviceTemplate.Domain.Primitives;

namespace PSL.MicroserviceTemplate.Domain.Templates;

public interface ITemplateRepository
{
    public Task<TemplateModel> GetTemplate(TemplateId templateId);
}
