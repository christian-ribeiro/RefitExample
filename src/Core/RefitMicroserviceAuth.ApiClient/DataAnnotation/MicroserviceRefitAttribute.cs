using RefitMicroserviceAuth.Arguments.Enum.Microservice;

namespace RefitMicroserviceAuth.ApiClient.DataAnnotation;

[AttributeUsage(AttributeTargets.Interface, Inherited = true)]
public class MicroserviceRefitAttribute(EnumMicroservice microservice) : Attribute
{
    public EnumMicroservice Microservice { get; private set; } = microservice;
}