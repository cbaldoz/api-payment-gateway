using System;
using PayMongo.Payment.Api.Domain.Entity;

namespace PayMongo.Payment.Api.Domain.Repository;

public interface IPaymentRepository
{
    Task<(bool result, string message)> SavePaymentAsync(PayMongoTransaction payment, CancellationToken cancellationToken = default);
    Task<(bool result, string message)> UpdatePaymentStatusAsync(PayMongoTransaction paymentStatus, CancellationToken cancellationToken = default);
    Task<(List<PaymentTransaction> transactions, string message)> GetPaymentTransactionAsync(
        int limit,
        DateTime startDate,
        DateTime endDate,
        string status,
        string transactionType,
        string account,
        CancellationToken cancellationToken = default);

    Task<(PaymentTransaction? transaction, string message)> GetTransactionByReferenceNumber(string referenceNumber, CancellationToken cancellationToken);
    Task<(PayMongoEvent PayMongoEvent, string message)> GetPayMongoEventByReferenceNumber(string referenceNumber, CancellationToken cancellationToken);
}
