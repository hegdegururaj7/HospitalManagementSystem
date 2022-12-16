using PSL.MicroserviceTemplate.Domain.Primitives;
using PSL.MicroserviceTemplate.Domain.Templates;
using System.Text.Json.Serialization;

namespace PSL.MicroserviceTemplate.Api.Controllers.Templates.v1;

public record GetTemplateContract
{
    public GetTemplateContract()
    {       
    }

    public GetTemplateContract(TemplateModel model)
    {
        Id = model.Id;
        Name= model.Name;
    }


    [JsonPropertyName("id")]
    public TemplateId Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; }
}
