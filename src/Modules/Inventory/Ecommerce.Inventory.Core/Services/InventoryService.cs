using Ecommerce.Inventory.Core.Repositories;
using Ecommerce.Inventory.Shared.Services;

namespace Ecommerce.Inventory.Core.Services;

public class InventoryService : IInventoryService
{
    private readonly IInventoryRepository _inventoryRepository;

    public InventoryService(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }
    
    public async Task<(bool IsStockReserved, string ErrorReason)> ReserveStock(Guid orderId, IEnumerable<(string Sku, int Quantity)> products)
    {
        var outOfStock = new List<string>();

        foreach (var product in products)
        {
            var inventory = await _inventoryRepository.Get(new (product.Sku));

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (inventory == null || inventory.Quantity < product.Quantity)
            {
                outOfStock.Add(product.Sku);
            }
        }

        if (outOfStock.Count > 0)
        {
            var reason = "Out of stock for product(s) " + outOfStock.Aggregate((sku1, sku2) => sku1 + "," + sku2);
            
            return (false, reason);
        }

        foreach (var product in products)
        {
            var inventory = await _inventoryRepository.Get(new (product.Sku));

            inventory.Reserve(product.Quantity);

            await _inventoryRepository.Update(inventory);
        }

        return (true, "Succeded");
    }

    public async Task ReleaseStock(Guid orderId, List<(string Sku, int Quantity)> products)
    {
        foreach (var product in products)
        {
            var inventory = await _inventoryRepository.Get(new (product.Sku));
            inventory.Release(product.Quantity);

            await _inventoryRepository.Update(inventory);
        }
    }
}