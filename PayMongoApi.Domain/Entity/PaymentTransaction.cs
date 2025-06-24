using System;

namespace PayMongo.Payment.Api.Domain.Entity;

public class PaymentTransaction
{
    public string PaymentIntentId { get; set; } = string.Empty;
    public string TransactionId { get; set; } = string.Empty;
    public string SessionId { get; set; } = string.Empty;
    public string ClientKey { get; set; } = string.Empty;
    public string Account { get; set; } = string.Empty;
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
    public string EventId { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
    public string RawPayload { get; set; } = string.Empty;
    public DateTime ReceivedAt { get; set; }
}
