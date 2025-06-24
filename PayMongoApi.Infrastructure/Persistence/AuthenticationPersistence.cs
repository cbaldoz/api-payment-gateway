using System;
using Dapper;
using MySql.Data.MySqlClient;
using PayMongo.Payment.Api.Domain.Entity;
using PayMongo.Payment.Api.Domain.Repository;
using PayMongo.Payment.Api.Infrastructure.Utility;

namespace PayMongo.Payment.Api.Infrastructure.Persistence;

public class AuthenticationPersistence : IAuthenticationRepository
{
    private readonly ConnStringConfig _connStringConfig;
    public AuthenticationPersistence(AppConfigProvider connStringConfig)
    {
        _connStringConfig = connStringConfig.ConnStringConfig ?? throw new ArgumentNullException(nameof(connStringConfig));
    }
    public async Task<AppModel> GetAppDetailsAsync(string sessionKey, CancellationToken cancellationToken = default)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        using (var cnn = new MySqlConnection(_connStringConfig.OmsAuthDbConnection))
        {
            await cnn.OpenAsync(cancellationToken);
            var query = @"select 
                id, 
                app_details_id, 
                access_token, 
                username, 
                user_type, 
                session_expiry, 
                request_details, 
                created_at 
            from 
                app_session 
            where 
                access_token = @access_token";
            var data = await cnn.QueryFirstOrDefaultAsync<AppModel>(query, new
            {
                access_token = sessionKey
            }, commandTimeout: 30);

            if (data is null)
                return new AppModel();
            else
                return data;
        }
    }
    public async Task<UserAccount> GetUserAccountAsync(string account, CancellationToken cancellationToken = default)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        using (var cnn = new MySqlConnection(_connStringConfig.OmsAccountDbConnection))
        {
            await cnn.OpenAsync(cancellationToken);
            var query = "SELECT account,email,account_code FROM account WHERE account = @account";
            var data = await cnn.QueryFirstOrDefaultAsync<UserAccount>(query, new
            {
                account
            }, commandTimeout: 30);

            if (data is null)
                return new UserAccount();
            else
                return data;
        }
    }
}
