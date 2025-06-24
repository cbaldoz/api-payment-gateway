using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using PayMongo.Payment.Api.Domain.Repository;
using PayMongo.Payment.Api.Infrastructure.Persistence;
using PayMongo.Payment.Api.Infrastructure.Utility;

namespace PayMongo.Payment.Api.Infrastructure;

public static class ServiceExtension
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        // Add your infrastructure services here
        services.AddSingleton<AppConfigProvider>();
        services.AddHttpClient<IPayMongoRepository, PayMongoPersistence>();
        services.AddScoped<IPaymentRepository, PaymentPersistence>();
        services.AddScoped<IAuthenticationRepository, AuthenticationPersistence>();
    }
}
