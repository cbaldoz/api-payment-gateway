using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PayMongo.Payment.Api.Application.Command;
using PayMongo.Payment.Api.Application.CommandHandler;
using PayMongo.Payment.Api.Application.Mapper;
using PayMongo.Payment.Api.Application.Query;
using PayMongo.Payment.Api.Application.QueryHandler;
using PayMongo.Payment.Api.Application.Service;
using PayMongo.Payment.Api.Application.Validation;
using PayMongo.Payment.Api.Domain.Entity;

namespace PayMongo.Payment.Api.Application;

public static class ServiceExtension
{
    public static void AddApplicationServices(this IServiceCollection service)
    {
        // Register AutoMapper
        service.AddAutoMapper(typeof(CommandCheckoutMapper));
        service.AddAutoMapper(typeof(CommandPaymentTransactionMapper));

        // Register MediatR
        service.AddScoped<IRequestHandler<InsertCheckoutSessionCommand, Result<bool>>, InsertCheckoutSessionCommandHandler>();
        service.AddScoped<IRequestHandler<CreateCheckoutSessionCommand, Result<CheckoutSession>>, CreateCheckoutSessionCommandHandler>();
        service.AddScoped<IRequestHandler<ExpiredCheckoutSessionCommand, Result<bool>>, ExpiredCheckoutSessionCommandHandler>();
        service.AddScoped<IRequestHandler<GetPaymentMethodsQuery, Result<List<string>>>, GetPaymentMethodsQueryHandler>();
        service.AddScoped<IRequestHandler<GetPaymentTransactionQuery, Result<List<PaymentTransaction>>>, GetPaymentTransactionQueryHandler>();
        service.AddScoped<IRequestHandler<GetTransactionByReferenceNumberQuery, Result<PaymentTransaction>>, GetTransactionByReferenceNumberQueryHandler>();

        // Register FluentValidation
        service.AddScoped<IValidator<CreateCheckoutSessionCommand>, CreateCheckoutSessionCommandValidator>();
        service.AddScoped<IValidator<GetTransactionByReferenceNumberQuery>, GetTransactionByReferenceNumberValidator>();

        // Service for ApplicationService
        service.AddScoped<IApplicationService, ApplicationService>();
    }
}
