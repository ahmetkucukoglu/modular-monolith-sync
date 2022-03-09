using Ecommerce.Inventory.Shared.Services;
using Ecommerce.Order.Core.Aggregates;
using Ecommerce.Order.Core.Repositories;
using Ecommerce.Payment.Shared.Services;
using Ecommerce.Shared.Abstractions.CQRS;
using Ecommerce.Shared.Abstractions.DDD;

namespace Ecommerce.Order.Core.Commands;

public record CreateOrder(Guid OrderId, IEnumerable<CreateOrderItem> Items) : ICommand;
public record CreateOrderItem(Guid OrderItemId, string Sku, int Quantity, decimal UnitPrice, string Currency);

public class CreateOrderHandler : ICommandHandler<CreateOrder>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IInventoryService _inventoryService;
    private readonly IPaymentService _paymentService;

    public CreateOrderHandler(IOrderRepository orderRepository, IInventoryService inventoryService, IPaymentService paymentService)
    {
        _orderRepository = orderRepository;
        _inventoryService = inventoryService;
        _paymentService = paymentService;
    }

    public async Task HandleAsync(CreateOrder command)
    {
        var order = await _orderRepository.Get(new (command.OrderId));

        if (order is not null)
            throw new DomainException("The order has already been added.");
        
        order = Order.Core.Aggregates.Order.Create(
            new(command.OrderId),
            command.Items.Select(i =>
                new OrderItem(new(i.OrderItemId), i.Sku, i.Quantity, new(i.UnitPrice, i.Currency))
            ));

        await _orderRepository.Create(order);

        var (isStockReserved, stockErrorReason) = await _inventoryService.ReserveStock(order.Id, order.Items.Select(i => (i.Sku, i.Quantity)).ToList());

        if (!isStockReserved)
        {
            order.Fail(stockErrorReason);
            await _orderRepository.Update(order);
            
            return;
        }

        var (isPaymentMade, paymentErrorReason) = await _paymentService.MakePayment(order.Id, order.TotalPrice.Amount, order.TotalPrice.Currency);

        if (!isPaymentMade)
        {
            await _inventoryService.ReleaseStock(order.Id, order.Items.Select(i => (i.Sku, i.Quantity)).ToList());
            order.Fail(paymentErrorReason);
            await _orderRepository.Update(order);
            
            return;
        }

        order.Pay();
        await _orderRepository.Update(order);
    }
}