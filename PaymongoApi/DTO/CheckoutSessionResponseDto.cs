using System;
using PayMongo.Payment.Api.Domain.Entity;

namespace PayMongo.Payment.Api.DTO;

public class CheckoutSessionResponseDto
{
    public string CheckoutUrl { get; set; } = string.Empty;
    public string ReferenceNumber { get; set; } = string.Empty;
    public double Amount { get; set; }
}