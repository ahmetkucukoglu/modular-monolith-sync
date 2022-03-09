using Ecommerce.Payment.Core.Repositories;
using Ecommerce.Payment.Core.ValueObjects;
using Ecommerce.Payment.Shared.Services;

namespace Ecommerce.Payment.Core.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IPaymentGateway _paymentGateway;

    public PaymentService(IPaymentRepository paymentRepository, IPaymentGateway paymentGateway)
    {
        _paymentRepository = paymentRepository;
        _paymentGateway = paymentGateway;
    }

    public async Task<(bool IsPaymentMade, string ErrorReason)> MakePayment(Guid orderId, decimal totalPrice, string currency)
    {
        var payment = Aggregates.Payment.Create(new OrderId(orderId), new Price(totalPrice, currency));
        
        await _paymentRepository.Create(payment);
        
        var result = _paymentGateway.Pay(new Price(totalPrice, currency));

        if (!result.IsSuccess)
        {
            payment.Fail(result.TransactionId, result.ErrorMessage);
            await _paymentRepository.Update(payment);

            return (false, result.ErrorMessage);
        }
        else
        {
            payment.Pay(result.TransactionId);
            await _paymentRepository.Update(payment);
        }

        return (true, "Succeded");
    }
}