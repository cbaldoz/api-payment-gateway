using System;
using MediatR;
using PayMongo.Payment.Api.Domain.Entity;

namespace PayMongo.Payment.Api.Application.Query;

public class GetTransactionByReferenceNumberQuery: IRequest<Result<PaymentTransaction>>
{
    public string ReferenceNumber { get; set; } = string.Empty;
}
