using System;
using MediatR;
using PayMongo.Payment.Api.Domain.Entity;

namespace PayMongo.Payment.Api.Application.Command;

public class CreateCheckoutSessionCommand : IRequest<Result<CheckoutSession>>
{
    public decimal Amount { get; set; }
    public string? Currency { get; set; }
    public List<string> PaymentMethodTypes { get; set; } = new List<string>();
    public string? Description { get; set; }
    public string? SuccessUrl { get; set; }
    public string? CancelUrl { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? MetadataTransactionId { get; set; }
    public List<LineItem> LineItems { get; set; } = new List<LineItem>();
    public bool SendEmailReceipt { get; set; } = true;
    public bool SendSmsReceipt { get; set; } = true;
    public bool ShowDescription { get; set; } = true;
    public bool ShowLineItems { get; set; } = true;
    public string AddressCity { get; set; } = string.Empty;
    public string AddressCountry { get; set; } = string.Empty;
    public string AddressLine1 { get; set; } = string.Empty;
    public string AddressLine2 { get; set; } = string.Empty;
    public string AddressPostalCode { get; set; } = string.Empty;
    public string AddressState { get; set; } = string.Empty;
    public class LineItem
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Images { get; set; } = new List<string>();
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
