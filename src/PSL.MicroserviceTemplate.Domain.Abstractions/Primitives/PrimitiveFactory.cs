namespace PSL.MicroserviceTemplate.Domain.Primitives;

public static class PrimitiveFactory
{
    public static IPrimitive Create(Type modelType, string value) => modelType.Name switch
    {
        // Guids
        nameof(TemplateId) => TemplateId.Parse(value),
        _ => throw new ArgumentOutOfRangeException(nameof(modelType), $"There is no match in the switch expression for '{modelType.Name}'")
    };
}
