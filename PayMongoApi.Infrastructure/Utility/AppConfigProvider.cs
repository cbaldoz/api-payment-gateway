using System;
using Microsoft.Extensions.Configuration;

namespace PayMongo.Payment.Api.Infrastructure.Utility;

public class AppConfigProvider
{
    private readonly IConfiguration _configuration;

    public AppConfigProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public PaymongoApiConfig PaymongoApi => GetPaymongoApiConfig();
    public ConnStringConfig ConnStringConfig => GetConnStringConfig();

    /// <summary>
    /// Retrieves the connection string configuration from the application settings.
    /// This method reads the connection strings for the OmsAuthDbConnection and PaymentDbConnection from the configuration.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private ConnStringConfig GetConnStringConfig()
    {
        var config = new ConnStringConfig();
        config.OmsAuthDbConnection = _configuration["ConnectionStrings:OmsAuthDbConnection"] ?? throw new InvalidOperationException("OmsAuthDbConnection is not configured.");
        config.OmsAccountDbConnection = _configuration["ConnectionStrings:OmsAccountDbConnection"] ?? throw new InvalidOperationException("OmsAccountDbConnection is not configured.");
        config.PaymentDbConnection = _configuration["ConnectionStrings:PaymentDbConnection"] ?? throw new InvalidOperationException("PaymentDbConnection is not configured.");
        return config;
    }

    /// <summary>
    /// Retrieves the Paymongo API configuration from the system defaults in the database.
    /// This method connects to the database using the connection string provided in the configuration and retrieves the Paymongo API settings.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private PaymongoApiConfig GetPaymongoApiConfig()
    {
        var config = new PaymongoApiConfig();
        var systemDefaults = PaymongoApiConfig.GetSystemDefaults(ConnStringConfig.PaymentDbConnection).Result;
        config.BaseUrl = systemDefaults.ContainsKey("base_url") ? systemDefaults["base_url"] : throw new InvalidOperationException("Paymongo API Base URL is not configured in system defaults.");
        config.SecretKey = systemDefaults.ContainsKey("secret_key") ? systemDefaults["secret_key"] : throw new InvalidOperationException("Paymongo API Secret Key is not configured in system defaults.");
        config.SuccessUrl = systemDefaults.ContainsKey("success_url") ? systemDefaults["success_url"] : throw new InvalidOperationException("Paymongo API Success URL is not configured in system defaults.");
        config.CancelUrl = systemDefaults.ContainsKey("cancel_url") ? systemDefaults["cancel_url"] : throw new InvalidOperationException("Paymongo API Cancel URL is not configured in system defaults.");
        config.PaymentLogo = systemDefaults.ContainsKey("payment_logo") ? systemDefaults["payment_logo"] : throw new InvalidOperationException("Paymongo API Payment Logo is not configured in system defaults.");
        config.PaymentDescription = systemDefaults.ContainsKey("payment_description") ? systemDefaults["payment_description"] : throw new InvalidOperationException("Paymongo API Payment Description is not configured in system defaults.");
        config.CreateCheckoutSession = _configuration["PaymongoApi:CheckoutSession:CreateCheckoutSession"] ?? throw new InvalidOperationException("Paymongo API Create Checkout Session URL is not configured.");
        config.ExpiredCheckoutSession = _configuration["PaymongoApi:CheckoutSession:ExpiredCheckoutSession"] ?? throw new InvalidOperationException("Paymongo API Expired Checkout Session URL is not configured.");
        config.GetPaymentMethods = _configuration["PaymongoApi:PaymentMethod:GetPaymentMethods"] ?? throw new InvalidOperationException("Paymongo API Get Payment Methods URL is not configured.");
        config.GetListOfPayments = _configuration["PaymongoApi:Payment:GetListOfPayments"] ?? throw new InvalidOperationException("Paymongo API Get List of Payment URL is not configured.");
        return config;
    }
}
