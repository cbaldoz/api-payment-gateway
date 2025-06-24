using System;
using MediatR;
using PayMongo.Payment.Api.Application.Query;
using PayMongo.Payment.Api.Domain.Repository;

namespace PayMongo.Payment.Api.Application.QueryHandler;

public class GetPaymentMethodsQueryHandler : IRequestHandler<GetPaymentMethodsQuery, Result<List<string>>>
{
    private readonly IPayMongoRepository _payMongoRepository;
    public GetPaymentMethodsQueryHandler(IPayMongoRepository payMongoRepository)
    {
        _payMongoRepository = payMongoRepository ?? throw new ArgumentNullException(nameof(payMongoRepository));
    }
    /// <summary>
    /// This method handles the GetPaymentMethodsQuery to retrieve available payment methods from the repository.
    /// </summary>
    /// <param name="getPaymentMethods"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<List<string>>> Handle(GetPaymentMethodsQuery getPaymentMethods, CancellationToken cancellationToken)
    {
        var paymentMethods = await _payMongoRepository.GetPaymentMethodsAsync(cancellationToken);
        return Result<List<string>>.Success(paymentMethods, "Payment methods retrieved successfully.");
    }
}