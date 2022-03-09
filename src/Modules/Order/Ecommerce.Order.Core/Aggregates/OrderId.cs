using Ecommerce.Shared.Abstractions.DDD;

namespace Ecommerce.Order.Core.Aggregates;

public class OrderId : EntityTypedId
{
    public OrderId()
    {
        
    }
    
    public OrderId(Guid id) : base(id)
    {
        
    }
}