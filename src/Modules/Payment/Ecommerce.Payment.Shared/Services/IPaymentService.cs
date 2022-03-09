namespace Ecommerce.Payment.Shared.Services;

public interface IPaymentService
{
    Task<(bool IsPaymentMade, string ErrorReason)> MakePayment(Guid orderId, decimal totalPrice, string currency);
}