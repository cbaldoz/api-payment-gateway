using System;
using AutoMapper;
using PayMongo.Payment.Api.Application.Command;
using PayMongo.Payment.Api.Domain.Entity;

namespace PayMongo.Payment.Api.Application.Mapper;

public class CommandCheckoutMapper : Profile
{
    public CommandCheckoutMapper()
    {
        /// <summary>
        /// Maps CreateCheckoutSessionCommand to CheckoutSession.
        /// </summary>
        CreateMap<CreateCheckoutSessionCommand, CheckoutSession>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => new CheckoutSession.CheckoutSessionData
            {
                Id = "", // or leave it empty
                Attributes = new CheckoutSession.CheckoutSessionAttributes
                {
                    Amount = src.Amount,
                    Currency = src.Currency ?? string.Empty,
                    Description = src.Description ?? string.Empty,
                    SuccessUrl = src.SuccessUrl ?? string.Empty,
                    CancelUrl = src.CancelUrl ?? string.Empty,
                    PaymentMethodTypes = src.PaymentMethodTypes ?? new List<string>(),
                    ReferenceNumber = src.ReferenceNumber ?? string.Empty,
                    SendEmailReceipt = src.SendEmailReceipt,
                    ShowDescription = src.ShowDescription,
                    ShowLineItems = src.ShowLineItems,
                    ReturnUrl = src.SuccessUrl ?? string.Empty, // Assuming ReturnUrl is the same as SuccessUrl
                    LineItems = (src.LineItems != null ? src.LineItems.Select(item => new CheckoutSession.LineItem
                    {
                        Amount = item.Amount,
                        Currency = item.Currency ?? string.Empty,
                        Description = item.Description ?? string.Empty,
                        Images = item.Images ?? new List<string>(),
                        Name = item.Name ?? string.Empty,
                        Quantity = item.Quantity
                    }).ToList() : new List<CheckoutSession.LineItem>()),
                    Billing = new CheckoutSession.BillingInfo
                    {
                        Email = src.Email ?? string.Empty,
                        Name = src.Name ?? string.Empty,
                        Phone = src.Phone ?? string.Empty,
                        Address = new CheckoutSession.Address()
                        {
                            City = src.AddressCity ?? string.Empty,
                            Country = src.AddressCountry ?? string.Empty,
                            Line1 = src.AddressLine1 ?? string.Empty,
                            Line2 = src.AddressLine2 ?? string.Empty,
                            PostalCode = src.AddressPostalCode ?? string.Empty,
                            State = src.AddressState ?? string.Empty
                        }
                    }
                    
                }
            }));
    }
}
