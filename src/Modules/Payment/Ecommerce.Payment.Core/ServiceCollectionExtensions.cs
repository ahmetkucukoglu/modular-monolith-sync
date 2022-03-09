using Ecommerce.Payment.Core.Services;
using Ecommerce.Payment.Shared.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Payment.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPaymentCore(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IPaymentService, PaymentService>();
        
        return serviceCollection;
    }
}