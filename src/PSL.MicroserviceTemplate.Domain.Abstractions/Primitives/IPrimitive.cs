namespace PSL.MicroserviceTemplate.Domain.Primitives;

public interface IPrimitive<out T> : IPrimitive
{
    T Value { get; }

    bool HasValue { get; }

    public Type ValueType => typeof(T);
}

public interface IPrimitive
{
}
