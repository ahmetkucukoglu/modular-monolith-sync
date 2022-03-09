namespace Ecommerce.Inventory.Shared.Services;

public interface IInventoryService
{
    Task<(bool IsStockReserved, string ErrorReason)> ReserveStock(Guid orderId, IEnumerable<(string Sku, int Quantity)> products);
    Task ReleaseStock(Guid orderId, List<(string Sku, int Quantity)> products);
}