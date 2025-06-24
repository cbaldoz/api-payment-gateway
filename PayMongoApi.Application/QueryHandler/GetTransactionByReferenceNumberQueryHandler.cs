using System;
using MediatR;
using PayMongo.Payment.Api.Application.Query;
using PayMongo.Payment.Api.Application.Validation;
using PayMongo.Payment.Api.Domain.Entity;
using PayMongo.Payment.Api.Domain.Repository;

namespace PayMongo.Payment.Api.Application.QueryHandler;

public class GetTransactionByReferenceNumberQueryHandler : IRequestHandler<GetTransactionByReferenceNumberQuery, Result<PaymentTransaction>>
{
    private readonly IPaymentRepository _paymentRepository;
    public GetTransactionByReferenceNumberQueryHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
    }

    /// <summary>
    /// Handles the retrieval of a payment transaction by its reference number.
    /// This method validates the request and queries the payment repository to fetch the transaction details.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<PaymentTransaction>> Handle(GetTransactionByReferenceNumberQuery request, CancellationToken cancellationToken)
    {
        var validationResult = new GetTransactionByReferenceNumberValidator().Validate(request);
        if (!validationResult.IsValid)
        {
            return Result<PaymentTransaction>.Failure(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));
        }
        var result = await _paymentRepository.GetTransactionByReferenceNumber(request.ReferenceNumber, cancellationToken);
        if (result.transaction == null)
            return Result<PaymentTransaction>.Failure(result.message);
        else
            return Result<PaymentTransaction>.Success(result.transaction);
    }
}
