using System;

namespace PayMongo.Payment.Api.Domain.Entity;

public class UserAccount
{
    public string Email { get; set; } = string.Empty;
    public string Account { get; set; } = string.Empty;
    public string AccountCode { get; set; } = string.Empty;
}
