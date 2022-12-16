using PSL.MicroserviceTemplate.Domain.Primitives;
using PSL.MicroserviceTemplate.Domain.Templates;

namespace PSL.MicroserviceTemplate.Data.Templates
{
    internal class TemplateRepository : ITemplateRepository
    {
        public Task<TemplateModel> GetTemplate(TemplateId templateId)
        {
            return Task.FromResult(new TemplateModel(templateId, $"Template {templateId}"));
        }
    }
}
