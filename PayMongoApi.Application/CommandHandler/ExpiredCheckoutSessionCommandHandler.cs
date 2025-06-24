using System;
using MediatR;
using PayMongo.Payment.Api.Application.Command;
using PayMongo.Payment.Api.Domain.Entity;
using PayMongo.Payment.Api.Domain.Repository;

namespace PayMongo.Payment.Api.Application.CommandHandler;

public class ExpiredCheckoutSessionCommandHandler: IRequestHandler<ExpiredCheckoutSessionCommand, Result<bool>>
{
    private readonly IPayMongoRepository _payMongoRepository;
    private readonly IPaymentRepository _paymentRepository;

    public ExpiredCheckoutSessionCommandHandler(IPayMongoRepository payMongoRepository, IPaymentRepository paymentRepository)
    {
        _payMongoRepository = payMongoRepository ?? throw new ArgumentNullException(nameof(payMongoRepository));
        _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
    }

    /// <summary>
    /// Handles the expiration of a checkout session by updating its status in the repository.
    /// This method is invoked when a checkout session has expired, marking it as such in the database.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<bool>> Handle(ExpiredCheckoutSessionCommand request, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetTransactionByReferenceNumber(request.SessionId, cancellationToken);
        if (payment.transaction == null)
        {
            return Result<bool>.Failure("Payment Transaction not found.");
        }
        var (result, message) = await _payMongoRepository.ExpiredCheckoutSession(payment.transaction.SessionId, cancellationToken);
        if (!result)
        {
            return Result<bool>.Failure(message);
        }
        else
        {
            var payMongoTransaction = new PayMongoTransaction();
            payMongoTransaction.Status = 3; // 3 represents cancel status
            payMongoTransaction.TransactionId = payment.transaction.TransactionId;
            var updateResult = await _paymentRepository.UpdatePaymentStatusAsync(payMongoTransaction, cancellationToken);
            if (!updateResult.result)
            {
                return Result<bool>.Failure("Failed to update transaction status to expired.");
            }
        }
        return Result<bool>.Success(true);
    }
}
