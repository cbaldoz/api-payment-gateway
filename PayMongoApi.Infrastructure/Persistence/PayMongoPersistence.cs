using System;
using System.Net.Http.Headers;
using System.Text.Json;
using PayMongo.Payment.Api.Domain.Entity;
using PayMongo.Payment.Api.Domain.Repository;
using PayMongo.Payment.Api.Infrastructure.Utility;

namespace PayMongo.Payment.Api.Infrastructure.Persistence;

public class PayMongoPersistence : IPayMongoRepository
{
    private readonly HttpClient _httpClient;
    private readonly PaymongoApiConfig _appConfigProvider;
    public PayMongoPersistence(HttpClient httpClient, AppConfigProvider appConfigProvider)
    {
        _httpClient = httpClient;
        _appConfigProvider = appConfigProvider.PaymongoApi ?? throw new InvalidOperationException("Paymongo API configuration is not set.");   
    }
    /// <summary>
    /// This method sends a POST request to the PayMongo API to create a checkout session.
    /// It serializes the provided CheckoutSession object to JSON and includes it in the request body.
    /// </summary>
    /// <param name="checkoutSession"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<(CheckoutSession? result, string message)> CreateCheckoutSession(CheckoutSession checkoutSession, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(checkoutSession);
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(_appConfigProvider.GetFullUrl(_appConfigProvider.CreateCheckoutSession)),
            Headers =
                        {
                            { "accept", "application/json" },
                            { "authorization", $"Basic {AuthUtils.ToBasicAuth(_appConfigProvider.SecretKey)}" },
                        },
            Content = new StringContent(json)
            {
                Headers =
                        {
                            ContentType = new MediaTypeHeaderValue("application/json")
                        }
            }
        };

        using (var response = await _httpClient.SendAsync(request, cancellationToken))
        {
            if (!response.IsSuccessStatusCode)
                return (null, $"Error: {response.StatusCode}, response: {await response.Content.ReadAsStringAsync()}");
            
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            var dataResponse = JsonSerializer.Deserialize<CheckoutSession>(body);
            if (dataResponse != null)
                return (dataResponse, "Checkout session created successfully.");
            else
                return (null, "Failed to deserialize response.");
        }
    }

    /// <summary>
    /// This method sends a POST request to the PayMongo API to expire a checkout session.
    /// It uses the session ID to construct the request URI and includes the necessary headers.
    /// </summary>
    /// <param name="sessionId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<(bool result, string message)> ExpiredCheckoutSession(string sessionId, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(_appConfigProvider.GetFullUrl(_appConfigProvider.ExpiredCheckoutSession.Replace("checkout_session_id", sessionId))),
            Headers =
                        {
                            { "accept", "application/json" },
                            { "authorization", $"Basic {AuthUtils.ToBasicAuth(_appConfigProvider.SecretKey)}" },
                        }
        };

        using (var response = await _httpClient.SendAsync(request, cancellationToken))
        {
            if (!response.IsSuccessStatusCode)
                return (false, $"Error: {response.StatusCode}, response: {await response.Content.ReadAsStringAsync()}");
            else
                return (true, "Checkout session expired successfully.");
        }
    }

    /// <summary>
    /// This method sends a GET request to the PayMongo API to fetch the payment methods.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<string>> GetPaymentMethodsAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(_appConfigProvider.GetFullUrl(_appConfigProvider.GetPaymentMethods)),
            Headers =
                        {
                            { "accept", "application/json" },
                            { "authorization", $"Basic {AuthUtils.ToBasicAuth(_appConfigProvider.SecretKey)}" },
                        }
        };

        using (var response = await _httpClient.SendAsync(request, cancellationToken))
        {
            if (!response.IsSuccessStatusCode)
                return new List<string> { $"Error: {response.StatusCode}, response: {await response.Content.ReadAsStringAsync()}" };
            
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            var dataResponse = JsonSerializer.Deserialize<List<string>>(body);
            if (dataResponse is not null)
                return dataResponse;
            else
                return new List<string>();
        }
    }
}
