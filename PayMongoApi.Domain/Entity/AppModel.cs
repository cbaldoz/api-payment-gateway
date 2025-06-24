using System;

namespace PayMongo.Payment.Api.Domain.Entity;

public class AppModel
{
    public string Id { get; set; } = string.Empty;
    public string AppDetailsId { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string UserType { get; set; } = string.Empty;
    public string RequestDetails { get; set; } = string.Empty;
    public DateTime SessionExpiry { get; set; }
    public DateTime CreatedAt { get; set; }
}
