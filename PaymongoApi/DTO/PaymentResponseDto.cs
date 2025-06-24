using System;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Transactions;
using PayMongo.Payment.Api.Domain.Entity;

namespace PayMongo.Payment.Api.DTO;

public class PaymentResponseDto
{
    public int TransactionType { get; set; }
    public int Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public string ReferenceNumber { get; set; } = string.Empty;
    public string PaymentIntentId { get; set; } = string.Empty;
    public DateTime TransactionCreatedAt { get; set; }
    public string TransactMethod { get; set; } = string.Empty;
    public string Last4Digit { get; set; } = string.Empty;
    public double AmountDeposited { get; set; }
    public double Fee { get; set; }
    public double NetAmount { get; set; }

    public List<PaymentResponseDto> Fill(List<PaymentTransaction> transactions)
    {
        var result = new List<PaymentResponseDto>();
        foreach (var transaction in transactions)
        {
            var data = new PaymentEventDto.PaymentAttributes();
            var source = new PaymentEventDto.Source();
            if (!string.IsNullOrEmpty(transaction.RawPayload))
            {
                var paymentEvent = JsonSerializer.Deserialize<PaymentEventDto>(transaction.RawPayload);
                if (paymentEvent?.Attributes?.Data?.Attributes != null)
                {
                    data = paymentEvent.Attributes.Data.Attributes;
                    if (paymentEvent.Attributes.Data.Attributes.Source != null)
                        source = paymentEvent.Attributes.Data.Attributes.Source;
                }
            }

            result.Add(new PaymentResponseDto
            {
                TransactionType = transaction.TransactionType,
                Status = transaction.Status,
                AmountDeposited = (double)(data.Amount is 0 ? transaction.Amount : data.Amount / 100),
                Fee = (double)(data.Fee / 100),
                NetAmount = (double)(data.NetAmount / 100),
                CreatedAt = transaction.SessionCreatedAt,
                ReferenceNumber = transaction.TransactionId,
                PaymentIntentId = transaction.PaymentIntentId,
                TransactionCreatedAt = transaction.TransactionCreatedAt,
                Last4Digit = source.Last4,
                TransactMethod = source.Type
            });
        }
        return result;
    }

    public PaymentResponseDto Fill(PaymentTransaction transaction)
    {
        var data = new PaymentEventDto.PaymentAttributes();
        var source = new PaymentEventDto.Source();
        if (!string.IsNullOrEmpty(transaction.RawPayload))
        {
            var paymentEvent = JsonSerializer.Deserialize<PaymentEventDto>(transaction.RawPayload);
            if (paymentEvent?.Attributes?.Data?.Attributes != null)
            {
                data = paymentEvent.Attributes.Data.Attributes;
                if (paymentEvent.Attributes.Data.Attributes.Source != null)
                    source = paymentEvent.Attributes.Data.Attributes.Source;
            }
        }

        var result = new PaymentResponseDto
        {
            TransactionType = transaction.TransactionType,
            Status = transaction.Status,
            AmountDeposited = (double)(data.Amount is 0 ? transaction.Amount : data.Amount / 100),
            Fee = (double)(data.Fee / 100),
            NetAmount = (double)(data.NetAmount / 100),
            CreatedAt = transaction.SessionCreatedAt,
            ReferenceNumber = transaction.TransactionId,
            PaymentIntentId = transaction.PaymentIntentId,
            TransactionCreatedAt = transaction.TransactionCreatedAt,
            Last4Digit = source.Last4,
            TransactMethod = source.Type
        };
        return result;
    }
}


