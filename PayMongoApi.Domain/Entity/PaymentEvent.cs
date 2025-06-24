using System;

namespace PayMongo.Payment.Api.Domain.Entity;

public class PayMongoEvent
{
    public string EventId { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
    public string TransactionId { get; set; } = string.Empty;
    public string RawPayload { get; set; } = string.Empty;
    public string PaymentIntentId { get; set; } = string.Empty;
    public DateTime ReceivedAt { get; set; }
}
