using System;

namespace PayMongo.Payment.Api.Infrastructure.Utility;

public class ConnStringConfig
{
    public string OmsAuthDbConnection { get; set; } = string.Empty;
    public string OmsAccountDbConnection { get; set; } = string.Empty;
    public string PaymentDbConnection { get; set; } = string.Empty;
}
