using System;
using PayMongo.Payment.Api.Application.Command;
using PayMongo.Payment.Api.Domain.Entity;
using PayMongo.Payment.Api.Infrastructure.Utility;

namespace PayMongo.Payment.Api.DTO;

public class CheckoutSessionDto
{
    public CreateCheckoutSessionCommand.LineItem FillLineItem(double amount)
    {
        return new CreateCheckoutSessionCommand.LineItem
        {
            Amount = (long)(amount * 100), // Convert to cents
            Currency = "PHP",
            Description = "PSE Connect Payment",
            Name = "Top-up",
            Quantity = 1,
            Images = ["https://example.com/image.png"]
        };
    }

    public CreateCheckoutSessionCommand Fill(
                double amount,
                string email,
                string account,
                List<string> paymentMethods,
                AppConfigProvider appConfigProvider)
    {
        var refNum = $"DEP{DateTime.UtcNow.ToString("yyMMddTHHmmssfff")}";
        return new CreateCheckoutSessionCommand
        {
            SuccessUrl = appConfigProvider.PaymongoApi.SuccessUrl,
            CancelUrl = appConfigProvider.PaymongoApi.CancelUrl.TrimEnd('/') + $"/{refNum}",
            Amount = (long)(amount * 100),
            Currency = "PHP",
            Description = "PSE Connect Checkout Session",
            LineItems = new List<CreateCheckoutSessionCommand.LineItem> { FillLineItem(amount) },
            Email = email,
            Name = account,
            ReferenceNumber = refNum,
            SendEmailReceipt = true,
            SendSmsReceipt = true,
            ShowDescription = true,
            ShowLineItems = true,
            PaymentMethodTypes = paymentMethods,
            AddressCountry = "PH",
        };
    }

    public InsertCheckoutSessionCommand FillTransaction(
        string referenceNumber,
        string account,
        double amount,
        string accountCode,
        CheckoutSession checkout)
    {
        var checkoutSession = checkout.Data.Attributes;
        return new InsertCheckoutSessionCommand
        {
            TransactionId = referenceNumber,
            SessionId = checkout.Data.Id ?? string.Empty,
            PaymentIntentId = checkoutSession.PaymentIntent.Id,
            ClientKey = checkoutSession.ClientKey,
            Account = account,
            AccountCode = accountCode,
            Amount = (decimal)amount,
            Currency = "PHP",
            Status = 0,
            TransactionType = 0,
            TransactionCreatedAt = DateTime.UtcNow.AddHours(8),
            TransactionUpdatedAt = DateTime.UtcNow.AddHours(8),
            CheckoutUrl = checkoutSession.CheckoutUrl,
            SessionCreatedAt = DateTimeOffset.FromUnixTimeSeconds(checkoutSession.CreatedAt).UtcDateTime.AddHours(8),
            SessionExpiresAt = DateTimeOffset.FromUnixTimeSeconds(checkoutSession.CreatedAt).UtcDateTime.AddHours(8).AddMinutes(30),
            CreditStatus = 0,
            JsonData = System.Text.Json.JsonSerializer.Serialize(checkout),
        };
    }
}
