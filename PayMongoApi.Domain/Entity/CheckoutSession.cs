using System;
using System.Text.Json.Serialization;

namespace PayMongo.Payment.Api.Domain.Entity;

public class CheckoutSession
{
    [JsonPropertyName("data")]
    public CheckoutSessionData Data { get; set; } = new CheckoutSessionData();
    public class CheckoutSessionData
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("type")]
        public string Type { get; set; } = "checkout_sessions";
        [JsonPropertyName("attributes")]
        public CheckoutSessionAttributes Attributes { get; set; } = new CheckoutSessionAttributes();
    }
    public class CheckoutSessionAttributes
    {
        [JsonPropertyName("billing")]
        public BillingInfo Billing { get; set; } = new BillingInfo();
        [JsonPropertyName("cancel_url")]
        public string CancelUrl { get; set; } = string.Empty;
        [JsonPropertyName("success_url")]
        public string SuccessUrl { get; set; } = string.Empty;
        [JsonPropertyName("checkout_url")]
        public string CheckoutUrl { get; set; } = string.Empty;
        [JsonPropertyName("client_key")]
        public string ClientKey { get; set; } = string.Empty;
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        [JsonPropertyName("line_items")]
        public List<LineItem> LineItems { get; set; } = new List<LineItem>();
        [JsonPropertyName("livemode")]
        public bool Livemode { get; set; }
        [JsonPropertyName("merchant")]
        public string Merchant { get; set; } = string.Empty;
        [JsonPropertyName("payments")]
        public List<Payment> Payments { get; set; } = new List<Payment>();
        [JsonPropertyName("payment_intent")]
        public PaymentIntent PaymentIntent { get; set; } = new PaymentIntent();
        [JsonPropertyName("payment_method_types")]
        public List<string> PaymentMethodTypes { get; set; } = new List<string>();
        [JsonPropertyName("reference_number")]
        public string ReferenceNumber { get; set; } = string.Empty;
        [JsonPropertyName("return_url")]
        public string ReturnUrl { get; set; } = string.Empty;
        [JsonPropertyName("send_email_receipt")]
        public bool SendEmailReceipt { get; set; }
        [JsonPropertyName("show_description")]
        public bool ShowDescription { get; set; }
        [JsonPropertyName("show_line_items")]
        public bool ShowLineItems { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;
        [JsonPropertyName("payment_status")]
        public string PaymentStatus { get; set; } = string.Empty;
        [JsonPropertyName("created_at")]
        public long CreatedAt { get; set; }
        [JsonPropertyName("updated_at")]
        public long UpdatedAt { get; set; }
        [JsonPropertyName("metadata")]
        public Metadata Metadata { get; set; } = new Metadata();
        [JsonPropertyName("payment_method")]
        public string PaymentMethod { get; set; } = string.Empty;
    }
    public class PaymentAttributes
    {
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
        [JsonPropertyName("balance_transaction_id")]
        public string BalanceTransactionId { get; set; } = string.Empty;
        [JsonPropertyName("billing")]
        public BillingInfo Billing { get; set; } = new BillingInfo();
        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        [JsonPropertyName("disputed")]
        public bool Disputed { get; set; }
        [JsonPropertyName("fee")]
        public string Fee { get; set; } = string.Empty;
        [JsonPropertyName("foreign_fee")]
        public string ForeignFee { get; set; } = string.Empty;
        [JsonPropertyName("livemode")]
        public bool Livemode { get; set; }
        [JsonPropertyName("net_amount")]
        public int NetAmount { get; set; }
        [JsonPropertyName("origin")]
        public string Origin { get; set; } = string.Empty;
        [JsonPropertyName("statement_descriptor")]
        public string StatementDescriptor { get; set; } = string.Empty;
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
        [JsonPropertyName("tax_amount")]
        public int TaxAmount { get; set; }
        [JsonPropertyName("taxes")]
        public List<Tax> Taxes { get; set; } = new List<Tax>();
        [JsonPropertyName("source")]
        public Source Source { get; set; } = new Source();
        [JsonPropertyName("metadata")]
        public Metadata Metadata { get; set; } = new Metadata();
        [JsonPropertyName("created_at")]
        public long CreatedAt { get; set; }
        [JsonPropertyName("paid_at")]
        public long PaidAt { get; set; }
    }
    public class BillingInfo
    {
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("phone")]
        public string Phone { get; set; } = string.Empty;
        [JsonPropertyName("address")]
        public Address Address { get; set; } = new Address();
    }
    public class Address
    {
        [JsonPropertyName("city")]
        public string City { get; set; } = string.Empty;
        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;
        [JsonPropertyName("line1")]
        public string Line1 { get; set; } = string.Empty;
        [JsonPropertyName("line2")]
        public string Line2 { get; set; } = string.Empty;
        [JsonPropertyName("postal_code")]
        public string PostalCode { get; set; } = string.Empty;
        [JsonPropertyName("state")]
        public string State { get; set; } = string.Empty;
    }
    public class LineItem
    {
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        [JsonPropertyName("images")]
        public List<string> Images { get; set; } = new List<string>();
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
    }
    public class Payment
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
        [JsonPropertyName("attributes")]
        public PaymentAttributes Attributes { get; set; } = new PaymentAttributes();
    }
    public class PaymentIntent
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
        [JsonPropertyName("attributes")]
        public PaymentIntentAttributes Attributes { get; set; } = new PaymentIntentAttributes();
    }
    public class PaymentIntentAttributes
    {
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
        [JsonPropertyName("payment_method_allowed")]
        public List<string> PaymentMethodAllowed { get; set; } = new List<string>();
        [JsonPropertyName("payments")]
        public List<Payment> Payments { get; set; } = new List<Payment>();
        [JsonPropertyName("metadata")]
        public Metadata Metadata { get; set; } = new Metadata();
        [JsonPropertyName("created_at")]
        public long CreatedAt { get; set; }
        [JsonPropertyName("updated_at")]
        public long UpdatedAt { get; set; }
    }
    public class Source
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
        [JsonPropertyName("brand")]
        public string Brand { get; set; } = string.Empty;
        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;
        [JsonPropertyName("last4")]
        public string Last4 { get; set; } = string.Empty;
    }
    public class Tax
    {
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;
        [JsonPropertyName("inclusive")]
        public bool Inclusive { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
        [JsonPropertyName("value")]
        public string Value { get; set; } = string.Empty;
    }
    public class Metadata
    {
        [JsonPropertyName("customer_number")]
        public string CustomerNumber { get; set; } = string.Empty;
        [JsonPropertyName("notes")]
        public string Notes { get; set; } = string.Empty;
        [JsonPropertyName("remarks")]
        public string Remarks { get; set; } = string.Empty;
    }
}
