using System;
using MediatR;

namespace PayMongo.Payment.Api.Application.Query;

public class GetPaymentMethodsQuery : IRequest<Result<List<string>>>
{
    
}
