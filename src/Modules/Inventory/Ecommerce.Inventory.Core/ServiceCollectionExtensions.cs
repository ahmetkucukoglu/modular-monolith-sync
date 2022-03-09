using Ecommerce.Inventory.Core.Services;
using Ecommerce.Inventory.Shared.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Inventory.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInventoryCore(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IInventoryService, InventoryService>();
        
        return serviceCollection;
    }
}