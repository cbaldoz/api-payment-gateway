using System;
using Dapper;
using MySql.Data.MySqlClient;

namespace PayMongo.Payment.Api.Infrastructure.Utility;

public class PaymongoApiConfig
{
    // Paymongo API Configuration
    // These properties are used to store the configuration details for the Paymongo API, such as base URL, secret key, public key, and various endpoints.
    public string BaseUrl { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string PublicKey { get; set; } = string.Empty;
    public string SuccessUrl { get; set; } = string.Empty;
    public string CancelUrl { get; set; } = string.Empty;

    // Paymongo API Endpoints
    // These endpoints are used to interact with the Paymongo API for creating checkout sessions, retrieving payment methods, etc.
    public string CreateCheckoutSession { get; set; } = string.Empty;
    public string ExpiredCheckoutSession { get; set; } = string.Empty;
    public string GetPaymentMethods { get; set; } = string.Empty;
    public string GetListOfPayments { get; set; } = string.Empty;

    public string PaymentLogo { get; set; } = string.Empty;
    public string PaymentDescription { get; set; } = string.Empty;

    // Method to get the full URL for a given endpoint
    /// <summary>
    /// Constructs the full URL for a given endpoint by combining the base URL and the endpoint.
    /// The method ensures that the base URL and endpoint are properly formatted, removing any trailing slashes from the base URL and leading slashes from the endpoint.
    /// </summary>
    /// <param name="endpoint"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public string GetFullUrl(string endpoint)
    {
        var fullUrl = $"{this.BaseUrl.TrimEnd('/')}/{endpoint.TrimStart('/')}";
        if (string.IsNullOrWhiteSpace(fullUrl))
            throw new InvalidOperationException("Base URL or endpoint configuration is invalid.");
        if (!Uri.TryCreate(fullUrl, UriKind.Absolute, out var requestUri))
            throw new InvalidOperationException("Invalid Base URL or endpoint configuration for Checkout Session.");
        return fullUrl;
    }

    /// <summary>
    /// Retrieves system default configurations from the database.
    /// This method connects to the database using the provided connection string and queries the `system_default` table for configurations related to Paymongo credentials and resources.
    /// It returns a dictionary containing the name and value of each configuration.
    /// </summary>
    /// <param name="connString">The connection string to the database.</param>
    /// <returns>A dictionary containing system default configurations.</returns>   
    public static async Task<Dictionary<string, string>> GetSystemDefaults(string connString)
    {
        var systemDefaults = new Dictionary<string, string>();
        var query = @"SELECT name,value FROM system_default WHERE category in ('paymongo_credentials','paymongo_resources')";
        using (var cnn = new MySqlConnection(connString))
        {
            await cnn.OpenAsync();
            var data = await cnn.QueryAsync(query);
            foreach (var item in data)
            {
                if (!string.IsNullOrWhiteSpace(item.name) && !string.IsNullOrWhiteSpace(item.value))
                {
                    systemDefaults[item.name] = item.value;
                }
            }
        }
        return systemDefaults;
    }
}