using System;
using System.Text;

namespace PayMongo.Payment.Api.Infrastructure.Utility;

public static class AuthUtils
{
    public static string ToBasicAuth(string key)
    {
        var keyBytes = Encoding.UTF8.GetBytes($"{key}:");
        return Convert.ToBase64String(keyBytes);
    }
}