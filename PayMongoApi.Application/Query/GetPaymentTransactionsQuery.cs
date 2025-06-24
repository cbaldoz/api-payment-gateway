using System;
using MediatR;
using PayMongo.Payment.Api.Domain.Entity;

namespace PayMongo.Payment.Api.Application.Query;

public class GetPaymentTransactionQuery : IRequest<Result<List<PaymentTransaction>>>
{
    public int Limit { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string TransactionType { get; set; } = string.Empty;
    public string Account { get; set; } = string.Empty;
}
