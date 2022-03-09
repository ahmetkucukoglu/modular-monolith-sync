using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Shared.DDD;

public static class ServiceCollectionExtensions
{
    // ReSharper disable once InconsistentNaming
    public static IServiceCollection AddDDD(this IServiceCollection serviceCollection, params Assembly[] assemblies)
    {
        return serviceCollection;
    } 
}