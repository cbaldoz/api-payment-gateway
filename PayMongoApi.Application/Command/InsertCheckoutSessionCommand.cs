using System;
using System.Net.NetworkInformation;
using MediatR;

namespace PayMongo.Payment.Api.Application.Command;

public class InsertCheckoutSessionCommand : IRequest<Result<bool>>
{
    public string PaymentIntentId { get; set; } = string.Empty;
    public string TransactionId { get; set; } = string.Empty;
    public string SessionId { get; set; } = string.Empty;
    public string ClientKey { get; set; } = string.Empty;
    public string Account { get; set; } = string.Empty;
    public string AccountCode { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public int Status { get; set; }
    public int TransactionType { get; set; }
    public DateTime TransactionCreatedAt { get; set; }
    public DateTime TransactionUpdatedAt { get; set; }
    public string CheckoutUrl { get; set; } = string.Empty;
    public DateTime SessionCreatedAt { get; set; }
    public DateTime SessionExpiresAt { get; set; }
    public int CreditStatus { get; set; }
    public string JsonData { get; set; } = string.Empty;
    public int ReconciledStatus { get; set; }
}
