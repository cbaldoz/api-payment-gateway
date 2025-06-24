using System;
using Dapper;
using MySql.Data.MySqlClient;
using PayMongo.Payment.Api.Infrastructure.Utility;

namespace PayMongo.Payment.Api.Infrastructure.Persistence;

public class PayMongoConfigPersistence
{
    public PaymongoApiConfig GetPaymongoApiConfig()
    {
        var result = new PaymongoApiConfig();
        var query = @"
            SELECT 
                category,
                name,
                value
            FROM PaymongoConfig
            WHERE Id = @Id";

        using (var connection = new MySqlConnection(new ConnStringConfig().OmsAuthDbConnection))
        {
            connection.Open();
            var queryResult = connection.QueryAsync<dynamic>(query);

            foreach (var item in queryResult.Result)
            {
                if (item.category == "paymongo_credentials")
                {
                    switch (item.name)
                    {
                        case "BaseUrl":
                            result.BaseUrl = item.value;
                            break;
                        case "SuccessUrl":
                            result.SuccessUrl = item.value;
                            break;
                        case "SecretKey":
                            result.SecretKey = item.value;
                            break;
                        case "CancelUrl":
                            result.CancelUrl = item.value;
                            break;
                        default:
                            throw new ArgumentException($"Unknown configuration name: {item.name}");
                    }
                }
                else if (item.category == "paymongo_fee")
                {
                    switch (item.name)
                    {
                        case "CreateCheckoutSession":
                            result.CreateCheckoutSession = item.value;
                            break;
                        case "ExpiredCheckoutSession":
                            result.ExpiredCheckoutSession = item.value;
                            break;
                        default:
                            throw new ArgumentException($"Unknown configuration name: {item.name}");
                    }
                }
                else if (item.category == "paymongo_payment")
                {
                    if (item.name == "GetListOfPayments")
                    {
                        result.GetListOfPayments = item.value;
                    }
                }
            }
        }
        return result;
    }
}
