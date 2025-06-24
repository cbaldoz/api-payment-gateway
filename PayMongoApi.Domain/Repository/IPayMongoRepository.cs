using System;
using PayMongo.Payment.Api.Domain.Entity;

namespace PayMongo.Payment.Api.Domain.Repository;

public interface IPayMongoRepository
{
    Task<(CheckoutSession? result, string message)> CreateCheckoutSession(CheckoutSession checkoutSession, CancellationToken cancellationToken = default);
    Task<(bool result, string message)> ExpiredCheckoutSession(string sessionId, CancellationToken cancellationToken = default);
    Task<List<string>> GetPaymentMethodsAsync(CancellationToken cancellationToken = default);
}
