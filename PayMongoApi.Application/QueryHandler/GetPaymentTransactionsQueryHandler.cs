using System;
using AutoMapper;
using MediatR;
using PayMongo.Payment.Api.Application.Query;
using PayMongo.Payment.Api.Domain.Entity;
using PayMongo.Payment.Api.Domain.Repository;

namespace PayMongo.Payment.Api.Application.QueryHandler;

public class GetPaymentTransactionQueryHandler : IRequestHandler<GetPaymentTransactionQuery ,Result<List<PaymentTransaction>>>
{
    private readonly IPaymentRepository _paymentRepository;
    public GetPaymentTransactionQueryHandler(IPaymentRepository paymentRepository, IMapper mapper)
    {
        _paymentRepository = paymentRepository;
    }
    /// <summary>
    /// Handles the retrieval of payment transactions based on the provided query parameters.
    /// This method queries the payment repository to fetch transactions within a specified date range, with optional filters for status and account.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<List<PaymentTransaction>>> Handle(GetPaymentTransactionQuery request, CancellationToken cancellationToken)
    {
        var result = await _paymentRepository.GetPaymentTransactionAsync(
            request.Limit,
            request.StartDate,
            request.EndDate,
            request.Status,
            request.TransactionType,
            request.Account,
            cancellationToken
        );
        return result.transactions.Any()
            ? Result<List<PaymentTransaction>>.Success(result.transactions, result.message)
            : Result<List<PaymentTransaction>>.Failure(result.message);
    }
}
