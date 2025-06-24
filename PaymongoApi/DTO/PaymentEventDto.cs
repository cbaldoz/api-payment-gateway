using System;
using System.Text.Json.Serialization;

namespace PayMongo.Payment.Api.DTO;

public class PaymentEventDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
    [JsonPropertyName("attributes")]
    public EventAttributes Attributes { get; set; } = new EventAttributes();
    public class EventAttributes
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
        [JsonPropertyName("livemode")]
        public bool LiveMode { get; set; }
        [JsonPropertyName("data")]
        public PaymentData Data { get; set; } = new PaymentData();
        [JsonPropertyName("previous_data")]
        public object PreviousData { get; set; } = string.Empty;
        [JsonPropertyName("created_at")]
        public long CreatedAt { get; set; }
        [JsonPropertyName("updated_at")]
        public long UpdatedAt { get; set; }
    }
    public class PaymentData
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
        [JsonPropertyName("attributes")]
        public PaymentAttributes Attributes { get; set; } = new PaymentAttributes();
    }
    public class PaymentAttributes
    {
        [JsonPropertyName("access_url")]
        public string AccessUrl { get; set; } = string.Empty;

        [JsonPropertyName("amount")]
        public long Amount { get; set; }

        [JsonPropertyName("balance_transaction_id")]
        public string BalanceTransactionId { get; set; } = string.Empty;

        [JsonPropertyName("billing")]
        public Billing Billing { get; set; } = new Billing();

        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("disputed")]
        public bool Disputed { get; set; }

        [JsonPropertyName("external_reference_number")]
        public string ExternalReferenceNumber { get; set; } = string.Empty;

        [JsonPropertyName("fee")]
        public long Fee { get; set; }

        [JsonPropertyName("foreign_fee")]
        public long ForeignFee { get; set; }

        [JsonPropertyName("instant_settlement")]
        public object InstantSettlement { get; set; } = string.Empty;

        [JsonPropertyName("livemode")]
        public bool Livemode { get; set; }

        [JsonPropertyName("net_amount")]
        public long NetAmount { get; set; }

        [JsonPropertyName("origin")]
        public string Origin { get; set; } = string.Empty;

        [JsonPropertyName("payment_intent_id")]
        public string PaymentIntentId { get; set; } = string.Empty;

        [JsonPropertyName("payout")]
        public object Payout { get; set; } = string.Empty;

        [JsonPropertyName("source")]
        public Source Source { get; set; } = new Source();

        [JsonPropertyName("statement_descriptor")]
        public string StatementDescriptor { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("tax_amount")]
        public object TaxAmount { get; set; } = string.Empty;

        [JsonPropertyName("promotion")]
        public object Promotion { get; set; } = string.Empty;

        [JsonPropertyName("refunds")]
        public List<object> Refunds { get; set; } = new List<object>();

        [JsonPropertyName("taxes")]
        public List<object> Taxes { get; set; } = new List<object>();

        [JsonPropertyName("available_at")]
        public long AvailableAt { get; set; }

        [JsonPropertyName("created_at")]
        public long? CreatedAt { get; set; }

        [JsonPropertyName("credited_at")]
        public long? CreditedAt { get; set; }

        [JsonPropertyName("paid_at")]
        public long? PaidAt { get; set; }

        [JsonPropertyName("updated_at")]
        public long UpdatedAt { get; set; }
    }
    public class Billing
    {
        [JsonPropertyName("address")]
        public Address Address { get; set; } = new Address();

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("phone")]
        public string Phone { get; set; } = string.Empty;
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
    public class Metadata
    {
        [JsonPropertyName("remarks")]
        public string Remarks { get; set; } = string.Empty;

        [JsonPropertyName("customer_number")]
        public string CustomerNumber { get; set; } = string.Empty;

        [JsonPropertyName("notes")]
        public string Notes { get; set; } = string.Empty;
    }
}
